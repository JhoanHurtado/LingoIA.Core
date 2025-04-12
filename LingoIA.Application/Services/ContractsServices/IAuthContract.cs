using LingoIA.Application.Dtos;

namespace LingoIA.Application.Services.ContractsServices
{
    public interface IAuthContract
    {
        Task<UserDto?> RegisterAsync(RegisterUserDto registerDto);
        Task<UserDto?> LoginAsync(LoginUserDto loginDto);
        Task<UserDto?> getUserByIDAsync(Guid userId);
    }
}
