using LingoIA.Domain.models;

namespace LingoIA.Infrastructure.Interfaces
{
    public interface ILanguageToolApiClient
    {
       public Task<CorrectionResult> CheckTextAsync(string text, string language);
    }
}