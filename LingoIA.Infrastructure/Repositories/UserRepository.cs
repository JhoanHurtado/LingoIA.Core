using LingoIA.Application.Interfaces;
using LingoIA.Domain.Entities;
using LingoIA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LingoIA.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LingoDbContext _dbContext;

        public UserRepository(LingoDbContext dbContext) => _dbContext = dbContext;

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.user.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _dbContext.user.FirstOrDefaultAsync(u => u.Id.Equals(userId)); 
        }

        public async Task<bool> saveAsync(User user)
        {
            await _dbContext.user.AddAsync(user);
            return Convert.ToBoolean(await _dbContext.SaveChangesAsync());
        }
    }
}