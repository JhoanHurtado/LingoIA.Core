using LingoIA.Domain.Entities;

namespace LingoIA.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid userId);
        Task<bool> saveAsync(User user);
    }
}