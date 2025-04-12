using AutoMapper;
using LingoIA.Application.Dtos;
using LingoIA.Application.Interfaces;
using LingoIA.Application.Services.ContractsServices;
using LingoIA.Application.Utils;
using LingoIA.Domain.Entities;

namespace LingoIA.Application.Services;

public class AuthService(IUserRepository userRepository, IMapper mapper): IAuthContract
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto?> RegisterAsync(RegisterUserDto registerDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser is not null) return null;

        var user = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            PreferredLanguage = registerDto.PreferredLanguage,
            Level = registerDto.Level,
            Password = registerDto.Password
        };

        var isSave = await _userRepository.saveAsync(user);
        if (isSave)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = JwtTokenGenerator.GenerateToken(user);
            return userDto;
        }
        return null;
    }

    public async Task<UserDto?> LoginAsync(LoginUserDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user is null || !user.VerifyPassword(loginDto.Password))
            return null;
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Token = JwtTokenGenerator.GenerateToken(user);
        return userDto;
    }

    public async Task<UserDto?> getUserByIDAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }
}
