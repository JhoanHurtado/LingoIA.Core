using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using LingoIA.Application.Dtos;
using LingoIA.Application.Interfaces;
using LingoIA.Domain.Enums;
using LingoIA.Domain.models;

namespace LingoIA.API.Sockets
{
    public class SocketServer
    {
        private readonly IServiceProvider _serviceProvider;
        private const int Port = 5050;

        public SocketServer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            TcpListener server = new TcpListener(IPAddress.Any, Port);
            server.Start();
            Console.WriteLine($"✅ Servidor iniciado en el puerto {Port}...");

            while (!cancellationToken.IsCancellationRequested)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(client));
            }

            server.Stop();
            Console.WriteLine("🛑 Servidor detenido.");
        }

        private async Task HandleClient(TcpClient client)
        {
            try
            {
                using NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[4096];
                int bytesRead = await stream.ReadAsync(buffer);

                if (bytesRead == 0)
                {
                    Console.WriteLine("⚠ Cliente se desconectó sin enviar datos.");
                    return;
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
                    return;
                }

                using var scope = _serviceProvider.CreateScope();
                var languagePracticeService = scope.ServiceProvider.GetRequiredService<ILanguagePracticeService>();

                string responseIA;

                if (message.ConversationId == null)
                {
                    responseIA = await languagePracticeService.StartConversationAsync("en", message.Text);
                }
                else
                {
                    responseIA = await languagePracticeService.SendMessageAsync(message.Text);
                }

                MessageAnalysis? result = languagePracticeService.GetLastAnalysis();


                // Preparar mensaje de respuesta
                message.CorrectedText = result?.Corrected;
                message.Score = result!.Score;
                message.CreatedAt = DateTime.UtcNow;
                message.AssistantResponse = responseIA;
                message.Explanation = result!.Explanation;
                message.Sender = EnumSender.IA;

                await SendResponseAsync(stream, message);

                Console.WriteLine($"✅ [Recibido] {message.Text} -> [Corregido] {result.Corrected} | Score: {message.Score:F2}%");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error en el manejo del cliente: {ex.Message}");
            }
            finally
            {
                //client.Close();
            }
        }

        private async Task SendResponseAsync(NetworkStream stream, object responseObj)
        {
            string json = JsonSerializer.Serialize(responseObj);
            byte[] data = Encoding.UTF8.GetBytes(json);
            await stream.WriteAsync(data);
        }
    }
}
