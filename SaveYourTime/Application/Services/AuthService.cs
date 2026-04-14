using WebApplication1.Application.DTOs;
using WebApplication1.Application.Interfaces;
using WebApplication1.Domain.Interface;
using WebApplication1.Domain.Object;
using WebApplication1.Infrastructure.Services;

namespace WebApplication1.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository) => _userRepository = userRepository;
    
    public async Task<AuthResponse> RegisterAsync(RegisterInput input)
    {
        if (await _userRepository.ExistsByUsernameAsync(input.Username))
            throw new Exception("Пользователь с таким именем уже существует");

        if (await _userRepository.ExistsByEmailAsync(input.Email))
            throw new Exception("Email уже зарегистрирован");

        var user = new User
        {
            Username = input.Username,
            Email = input.Email,
            PasswordHash = PasswordHasher.Hash(input.Password)
        };

        var createdUser = await _userRepository.CreateAsync(user);

        return new AuthResponse(
            createdUser.Id,
            createdUser.Username,
            createdUser.Email,
            DateTime.UtcNow.AddDays(7));
    }


    public async Task<AuthResponse> LoginAsync(LoginInput input)
    {
        
        var user = await _userRepository.GetByUsernameAsync(input.UsernameOrEmail)
                   ?? await _userRepository.GetByEmailAsync(input.UsernameOrEmail);
        
        if (user == null)
            throw new Exception("Неверный логин или пароль");
        
        if (!PasswordHasher.Verify(input.Password, user.PasswordHash))
            throw new Exception("Неверный логин или пароль");

        return new AuthResponse(
            user.Id,
            user.Username,
            user.Email,
            DateTime.UtcNow.AddDays(7));
    }
}