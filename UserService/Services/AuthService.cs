using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;
using UserService.Repositories;

namespace UserService.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly AppDbContext _dbContext;

    public AuthService(UserRepository userRepository, TokenService tokenService, AppDbContext appDbContext)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _dbContext = appDbContext;
    }

    public ServiceResponse<string> RegisterUser(RegisterDto registerDto)
    {
        var response = new ServiceResponse<string>();

        var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == registerDto.Email);
        if (existingUser != null)
        {
            response.Success = false;
            response.Message = "User already exists.";
            return response;
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();

        response.Success = true;
        response.Message = "User registered successfully.";
        return response;
    }



    public ServiceResponse<AuthResponseDto> ValidateUser(LoginDto loginDto)
    {
        var response = new ServiceResponse<AuthResponseDto>();

        var user = _userRepository.GetUserByEmail(loginDto.Email);

        if (user == null)
        {
            response.Success = false;
            response.Message = "User not found.";
            return response;
        }

        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            response.Success = false;
            response.Message = "Invalid credentials.";
            return response;
        }

        var token = _tokenService.GenerateToken(user.Id.ToString(), user.Username);

        response.Success = true;
        response.Data = new AuthResponseDto
        {
            Token = token,
            Username = user.Username
        };

        return response;
    }

}
