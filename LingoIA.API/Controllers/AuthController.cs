using LingoIA.Application.Dtos;
using LingoIA.Application.Services;
using LingoIA.Application.Services.ContractsServices;
using Microsoft.AspNetCore.Mvc;

namespace LingoIA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
    private readonly IAuthContract _authService;

    public AuthController(IAuthContract authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        return result is not null ? Ok(result) : BadRequest("El usuario ya existe");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return result is not null ? Ok(result) : Unauthorized("Credenciales incorrectas");
    }
    }
}