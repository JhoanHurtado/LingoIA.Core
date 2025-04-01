using Microsoft.EntityFrameworkCore;
using LingoIA.Domain.Entities;

namespace LingoIA.Infrastructure.Persistence
{
    public class LingoDbContext : DbContext
    {
        public LingoDbContext(DbContextOptions<LingoDbContext> options) : base(options) { }

        public DbSet<User> user { get; set; }
        public DbSet<Conversation> conversations { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<LearningData> LearningData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Relación User → Conversations (1:N)
            modelBuilder.Entity<Conversation>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Conversation → Messages (1:N)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
