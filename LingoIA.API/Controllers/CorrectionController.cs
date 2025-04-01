using LingoIA.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LingoIA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CorrectionController : ControllerBase
    {
        private readonly ITextCorrectionService _textCorrectionService;

        public CorrectionController(ITextCorrectionService textCorrectionService)
        {
            _textCorrectionService = textCorrectionService;
        }

        [HttpPost]
        public async Task<IActionResult> CorrectText([FromBody] CorrectionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest(new { message = "El texto no puede estar vacío." });
            }

            var correctedText = await _textCorrectionService.CorrectTextAsync(request.Text, request.language);
            return Ok(new { correctedText });
        }
    }


    public class CorrectionRequest
    {
        public required string Text { get; set; }
        public required string language { get; set; }
    }
}