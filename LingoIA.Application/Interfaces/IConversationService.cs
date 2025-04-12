using LingoIA.Domain.Entities;

namespace LingoIA.Application.Interfaces
{
    public interface IConversationService
    {
        Task<IEnumerable<Conversation>> GetAllConversationsAsync(Guid userId);
        Task<Conversation?> GetConversationByIdAsync(Guid conversationId);
        Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(Guid conversationId);
        Task<Message?> GetMessageByIdAsync(Guid messageId);
        Task<(Conversation conversation, List<Message> messages)> GetConversationWithMessagesAsync(Guid conversationId);
        Task<Message> AddMessageAsync(Guid conversationId, List<Message> message);
        Task<Conversation> AddConversationAsync(Conversation conversation);
        Task<bool> DeleteMessageAsync(Guid messageId);
        Task<bool> DeleteConversationAsync(Guid conversationId);
    }
}
