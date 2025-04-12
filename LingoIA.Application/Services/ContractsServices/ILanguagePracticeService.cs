using LingoIA.Domain.Entities;
using LingoIA.Domain.models;

namespace LingoIA.Application.Services.ContractsServices
{
    public interface ILanguagePracticeService
    {
        Task<string> StartConversationAsync(string language, string topic, string username);
        Task<string> SendMessageAsync(string message, List<Dictionary<string, string>> historyMessage, string username);
        public MessageAnalysis? GetLastAnalysis();
        List<Dictionary<string, string>> GetMessageHistory();

        void Reset();
    }
}
