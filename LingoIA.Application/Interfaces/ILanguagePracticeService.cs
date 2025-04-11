using LingoIA.Domain.models;

namespace LingoIA.Application.Interfaces
{
    public interface ILanguagePracticeService
    {
        Task<string> StartConversationAsync(string language, string topic);
        Task<string> SendMessageAsync(string message);
        public MessageAnalysis? GetLastAnalysis();
        void Reset();
    }
}
