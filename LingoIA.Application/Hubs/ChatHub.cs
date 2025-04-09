using LingoIA.Application.Dtos;
using Microsoft.AspNetCore.SignalR;


namespace LingoIA.Application.Hubs
{

    // Dominio
    public class ChatMessage
    {
        public string User { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }


    public class ChatHub : Hub
    {
        public async Task SendMessage(MessageDto message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message.User, message.Text);
        }
    }
}