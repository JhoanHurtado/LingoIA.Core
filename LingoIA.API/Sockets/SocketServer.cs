using AutoMapper;
using LingoIA.Application.Dtos;
using LingoIA.Application.Interfaces;
using LingoIA.Application.Services.ContractsServices;
using LingoIA.Domain.Entities;
using LingoIA.Domain.Enums;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public class SocketServer
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const int Port = 5050;
    private List<Message> messages = new List<Message>();
    private bool isNewChat = true;

    public SocketServer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        TcpListener server = new TcpListener(IPAddress.Any, Port);
        server.Start();
        Console.WriteLine($"✅ Servidor iniciado en el puerto {Port}...");

        while (!cancellationToken.IsCancellationRequested)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClientAsync(client));
        }

        server.Stop();
        Console.WriteLine("🛑 Servidor detenido.");
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using var scope = _scopeFactory.CreateScope();

        var authService = scope.ServiceProvider.GetRequiredService<IAuthContract>();
        var conversationService = scope.ServiceProvider.GetRequiredService<IConversationService>();
        var languagePracticeService = scope.ServiceProvider.GetRequiredService<ILanguagePracticeService>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        var stream = client.GetStream();
        byte[] buffer = new byte[4096];

        try
        {
            while (true)
            {
                int bytesRead = await stream.ReadAsync(buffer);

                if (bytesRead == 0)
                {
                    Console.WriteLine("⚠ Cliente desconectado.");
                    break;
                }

                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"📥 Solicitud recibida: {request}");

                var message = JsonSerializer.Deserialize<MessageDto>(request, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (message == null)
                {
                    Console.WriteLine("❌ No se pudo deserializar el mensaje.");
                    continue;
                }

                var user = await authService.getUserByIDAsync(message.User);
                var conversationDTO = new ConversationDto();
                string responseIA;

                if (message.ConversationId == null)
                {
                    conversationDTO = new ConversationDto
                    {
                        Language = message.Language,
                        Topic = "",
                        UserId = message.User
                    };

                    var conversation = mapper.Map<Conversation>(conversationDTO);
                    var newConversation = await conversationService.AddConversationAsync(conversation);
                    conversationDTO.Id = newConversation.Id;

                    responseIA = await languagePracticeService.StartConversationAsync(message.Language, message.Text, user?.Name ?? "Usuario");
                }
                else
                {
                    isNewChat = false;
                    var conversation = await conversationService.GetConversationWithMessagesAsync((Guid)message.ConversationId);
                    conversationDTO = mapper.Map<ConversationDto>(conversation.conversation);
                    var _messageHistory = conversation.messages.Select(m => new Dictionary<string, string>
                    {
                        { "role", m.Role },
                        { "content", m.Content }
                    }).ToList();

                    responseIA = await languagePracticeService.SendMessageAsync(message.Text, _messageHistory, user?.Name ?? "Usuario");
                }

                var result = languagePracticeService.GetLastAnalysis();
                var conversationHistory = languagePracticeService.GetMessageHistory();
                if (!isNewChat)
                {
                    var lastMessage = conversationHistory.LastOrDefault();
                    messages = new List<Message>
                    {
                        new Message
                        {
                            Text = message.Text,
                            Role = lastMessage["role"],
                            Content = lastMessage["content"],
                            Sender = EnumSender.User,
                            ConversationId = conversationDTO.Id
                        }
                    };

                }
                else
                {
                    messages = conversationHistory.Select(msg => new Message
                    {
                        Text = message.Text,
                        Role = msg["role"],
                        Content = msg["content"],
                        Sender = EnumSender.User,
                        ConversationId = conversationDTO.Id
                    }).ToList();
                }

                await conversationService.AddMessageAsync(conversationDTO.Id, messages);

                message.CorrectedText = result?.Corrected!;
                message.Score = result?.Score ?? 0;
                message.CreatedAt = DateTime.UtcNow;
                message.AssistantResponse = responseIA;
                message.Explanation = result?.Explanation!;
                message.ConversationId = conversationDTO.Id;
                message.Sender = EnumSender.IA;

                await SendResponseAsync(stream, message);

                Console.WriteLine($"✅ [Recibido] {message.Text} -> [Corregido] {result?.Corrected} | Score: {message.Score:F2}%");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"🚨 Error en el manejo del cliente: {ex.Message}");
        }
        finally
        {
            stream.Close();
            client.Close();
        }
    }

    private async Task SendResponseAsync(NetworkStream stream, object responseObj)
    {
        string json = JsonSerializer.Serialize(responseObj);
        byte[] data = Encoding.UTF8.GetBytes(json);
        await stream.WriteAsync(data, 0, data.Length);
    }
}
