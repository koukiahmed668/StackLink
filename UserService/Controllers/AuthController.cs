using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Services;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto registerDto)
    {
        var response = _authService.RegisterUser(registerDto);
        if (!response.Success) return Conflict(response.Message);

        return Ok(response.Message);
    }


    [HttpPost("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        var response = _authService.ValidateUser(loginDto);

        if (!response.Success)
            return Unauthorized(response.Message);

        return Ok(response.Data);
    }

}
