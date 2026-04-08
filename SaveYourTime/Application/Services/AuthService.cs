using WebApplication1.Application.DTOs;
using WebApplication1.Application.Interfaces;
using WebApplication1.Domain.Interface;
using WebApplication1.Domain.Object;
using WebApplication1.Infrastructure.Services;

namespace WebApplication1.Application.Services;
using Microsoft.Extensions.Logging;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterInput input)
    {
        _logger.LogInformation("Registration attempt for: {Username}", input.Username);

       
        if (await _userRepository.ExistsByUsernameAsync(input.Username))
            throw new Exception("Пользователь с таким именем уже существует");

 
        if (await _userRepository.ExistsByEmailAsync(input.Email))
            throw new Exception("Email уже зарегистрирован");

     
        var passwordHash = PasswordHasher.Hash(input.Password);

       
        var user = new User
        {
            Username = input.Username,
            Email = input.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            RoleId = 2 
        };

      
        var createdUser = await _userRepository.CreateAsync(user);

        _logger.LogInformation("User registered successfully: {UserId}", createdUser.Id);

        return new AuthResponse(
            createdUser.Id,
            createdUser.Username,
            createdUser.Email,
            DateTime.UtcNow.AddDays(7),
            createdUser.Role?.Name
        );
    }

    public async Task<AuthResponse> LoginAsync(LoginInput input)
    {
        _logger.LogInformation("Login attempt for: {UsernameOrEmail}", input.UsernameOrEmail);

        
        var user = await _userRepository.GetByUsernameAsync(input.UsernameOrEmail);
        
        if (user == null)
            user = await _userRepository.GetByEmailAsync(input.UsernameOrEmail);
 
        if (user == null)
        {
            _logger.LogWarning("Login failed: user not found");
            throw new Exception("Неверный логин или пароль");
        }

      
        if (!PasswordHasher.Verify(input.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: wrong password");
            throw new Exception("Неверный логин или пароль");
        }

      
        await _userRepository.UpdateLastLoginAsync(user.Id);

        _logger.LogInformation("User logged in successfully: {UserId}", user.Id);

        return new AuthResponse(
            user.Id,
            user.Username,
            user.Email,
            DateTime.UtcNow.AddDays(7),
            user.Role?.Name
        );
    }

    public async Task LogoutAsync(int userId)
    {
        _logger.LogInformation("User logged out: {UserId}", userId);
        await Task.CompletedTask;
    }
}