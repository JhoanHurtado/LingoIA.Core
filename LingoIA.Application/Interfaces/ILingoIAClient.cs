using LingoIA.Domain.Entities;
using LingoIA.Domain.models;

namespace LingoIA.Application.Interfaces
{
    public interface ILingoIAClient
    {
        Task<string> StartNewConversation(string language, string topic, string username);
        Task<string> SendMessage(string message, List<Dictionary<string, string>> historyMessage, string username);
        MessageAnalysis? GetLastAnalysis();
        List<Dictionary<string, string>> GetMessageHistory();
        void ResetConversation();
    }
}
