using LingoIA.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LingoIA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly ILanguagePracticeService _languagePracticeService;

        public ConversationController(ILanguagePracticeService service)
        {
            _languagePracticeService = service;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromQuery] string language, [FromQuery] string topic)
        {
            var response = await _languagePracticeService.StartConversationAsync(language, topic);
            return Ok(response);
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] string message)
        {
            var response = await _languagePracticeService.SendMessageAsync(message);
            return Ok(response);
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _languagePracticeService.Reset();
            return Ok("Conversación reiniciada.");
        }
    }
}
