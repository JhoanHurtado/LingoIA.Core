using LingoIA.Domain.Entities;

namespace LingoIA.Infrastructure.Interfaces
{
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> saveAsync(User user);
}

}