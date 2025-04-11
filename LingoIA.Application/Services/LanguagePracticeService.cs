using LingoIA.Application.Interfaces;
using LingoIA.Domain.Interfaces;
using LingoIA.Domain.models;

namespace LingoIA.Application.Services
{
    public class LanguagePracticeService : ILanguagePracticeService
    {
        private readonly ILingoIAClient _lingoIAClient;

        public LanguagePracticeService(ILingoIAClient lingoIAClient)
        {
            _lingoIAClient = lingoIAClient;
        }

        public Task<string> StartConversationAsync(string language, string topic)
        {
            return _lingoIAClient.StartNewConversation(language, topic);
        }

        public Task<string> SendMessageAsync(string message)
        {
            return _lingoIAClient.SendMessage(message);
        }

        public MessageAnalysis? GetLastAnalysis()
        {
            return _lingoIAClient.GetLastAnalysis();
        }

        public void Reset()
        {
            _lingoIAClient.ResetConversation();
        }
    }

}
