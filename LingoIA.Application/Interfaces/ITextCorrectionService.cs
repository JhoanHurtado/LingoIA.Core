using LingoIA.Domain.models;

namespace LingoIA.Application.Interfaces
{
public interface ITextCorrectionService
{
    Task<CorrectionResult> CorrectTextAsync(string text, string language);
}
}