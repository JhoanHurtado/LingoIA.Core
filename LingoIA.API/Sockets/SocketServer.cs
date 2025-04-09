using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using LingoIA.Application.Dtos;
using LingoIA.Application.Interfaces;
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

        public async Task StartAsync()
        {
            TcpListener server = new TcpListener(IPAddress.Any, Port);
            server.Start();
            Console.WriteLine($"Servidor iniciado en el puerto {Port}...");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(client));
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var parts = request.Split('|', 2);
            var message = JsonSerializer.Deserialize<MessageDto>(request);
            if (parts.Length < 2)
            {
                await stream.WriteAsync(Encoding.UTF8.GetBytes("Formato incorrecto"));
                return;
            }

            string language = parts[0].Trim();
            string text = parts[1].Trim();

            // 🔹 Crear un scope para obtener el servicio Scoped
            using (var scope = _serviceProvider.CreateScope())
            {
                var textCorrectionService = scope.ServiceProvider.GetRequiredService<ITextCorrectionService>();
                CorrectionResult result = await textCorrectionService.CorrectTextAsync(text, language);

                string response = $"Texto corregido: {result.CorrectedText}\nErrores:\n{string.Join("\n", result.Errors)}";
                await stream.WriteAsync(Encoding.UTF8.GetBytes(response));
                Console.WriteLine($"[Recibido] {text} -> [Corregido] {result.CorrectedText}");
            }
        }
    }
}