using LingoIA.Application.Interfaces;
using LingoIA.Domain.models;
using LingoIA.Infrastructure.Interfaces;

namespace LingoIA.Application.Services.LanguageTool
{
    public class TextCorrectionService : ITextCorrectionService
    {
        private readonly ILanguageToolApiClient _languageToolClient;

        public TextCorrectionService(ILanguageToolApiClient languageToolClient)
        {
            _languageToolClient = languageToolClient;
        }

        public async Task<CorrectionResult> CorrectTextAsync(string text, string language)
        {
            var result = await _languageToolClient.CheckTextAsync(text, language);
            return result;
        }
    }
}