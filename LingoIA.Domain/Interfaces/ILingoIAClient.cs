using LingoIA.Domain.models;

namespace LingoIA.Domain.Interfaces
{
    public interface ILingoIAClient
    {
        Task<string> StartNewConversation(string language, string topic);
        Task<string> SendMessage(string message);
        MessageAnalysis? GetLastAnalysis();
        void ResetConversation(); // para iniciar una nueva
    }
}
