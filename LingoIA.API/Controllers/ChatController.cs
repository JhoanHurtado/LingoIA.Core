using LingoIA.Application.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LingoIA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _chatHub;

        public ChatController(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }

        [HttpPost("send")] // Endpoint para enviar mensaje por API
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", message.User, message.Message);
            return Ok(new { status = "Message sent" });
        }
    }
}