using LingoIA.Application.Interfaces;
using LingoIA.Domain.Entities;
using LingoIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LingoIA.Infrastructure.Repositories
{
    public class ConversationService(LingoDbContext context) : IConversationService
    {
        private readonly LingoDbContext _dbContext = context;

        public async Task<IEnumerable<Conversation>> GetAllConversationsAsync(Guid userId)
        {
            return await _dbContext.conversations
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Conversation?> GetConversationByIdAsync(Guid conversationId)
        {
            return await _dbContext.conversations.FindAsync(conversationId);
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(Guid conversationId)
        {
            return await _dbContext.messages
                .Where(m => m.ConversationId == conversationId)
                .ToListAsync();
        }

        public async Task<Message?> GetMessageByIdAsync(Guid messageId)
        {
            return await _dbContext.messages.FindAsync(messageId);
        }

        public async Task<(Conversation conversation, List<Message> messages)> GetConversationWithMessagesAsync(Guid conversationId)
        {
            var conversation = await _dbContext.conversations.FindAsync(conversationId);
            var messages = await _dbContext.messages
                .Where(m => m.ConversationId == conversationId)
                .ToListAsync();

            if (conversation is null)
                throw new KeyNotFoundException("Conversation not found");

            return (conversation, messages);
        }

        public async Task<Message> AddMessageAsync(Guid conversationId, List<Message> message)
        {
            message.FirstOrDefault().ConversationId = conversationId;
            await _dbContext.messages.AddRangeAsync(message);
            await _dbContext.SaveChangesAsync();
            return message.FirstOrDefault();
        }

        public async Task<bool> DeleteMessageAsync(Guid messageId)
        {
            var message = await _dbContext.messages.FindAsync(messageId);
            if (message is null) return false;

            _dbContext.messages.Remove(message);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteConversationAsync(Guid conversationId)
        {
            var conversation = await _dbContext.conversations.FindAsync(conversationId);
            if (conversation is null) return false;

            var messages = await _dbContext.messages
                .Where(m => m.ConversationId == conversationId)
                .ToListAsync();

            _dbContext.messages.RemoveRange(messages);
            _dbContext.conversations.Remove(conversation);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Conversation> AddConversationAsync(Conversation conversation)
        {
            _dbContext.conversations.Add(conversation);
            await _dbContext.SaveChangesAsync();
            return conversation;
        }
    }
}
