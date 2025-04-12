using LingoIA.Application.Services.ContractsServices;
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
        public async Task<IActionResult> Start([FromQuery] string language, [FromQuery] string topic, string name)
        {
            var response = await _languagePracticeService.StartConversationAsync(language, topic, name);
            return Ok(response);
        }

        //[HttpPost("send")]
        //public async Task<IActionResult> Send([FromBody] string message,List<Dictionary<string, string>> historyMessage, string name)
        //{
        //    var response = await _languagePracticeService.SendMessageAsync(message, historyMessage, name);
        //    return Ok(response);
        //}

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _languagePracticeService.Reset();
            return Ok("Conversación reiniciada.");
        }
    }
}
