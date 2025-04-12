using LingoIA.Application.Interfaces;
using LingoIA.Application.Services.ContractsServices;
using LingoIA.Domain.Entities;
using LingoIA.Domain.models;

namespace LingoIA.Application.Services
{
    public class LanguagePracticeService(ILingoIAClient lingoIAClient) : ILanguagePracticeService
    {
        private readonly ILingoIAClient _lingoIAClient = lingoIAClient;

        public Task<string> StartConversationAsync(string language, string topic, string username)
        {
            return _lingoIAClient.StartNewConversation(language, topic, username);
        }

        public Task<string> SendMessageAsync(string message, List<Dictionary<string, string>> historyMessage,string username)
        {
            return _lingoIAClient.SendMessage(message, historyMessage, username);
        }

        public MessageAnalysis? GetLastAnalysis()
        {
            return _lingoIAClient.GetLastAnalysis();
        }

        public void Reset()
        {
            _lingoIAClient.ResetConversation();
        }

        public List<Dictionary<string, string>> GetMessageHistory() => _lingoIAClient.GetMessageHistory();
    }

}
