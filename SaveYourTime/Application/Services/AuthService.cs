using WebApplication1.Domain.Interfaces.Services;
using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Repositories;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.Utils;

namespace WebApplication1.Application.Services;


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public AuthService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<UserResponse> RegisterAsync(UserInput input)
    {
        if (await _userRepository.ExistsByUsernameAsync(input.Username))
            throw new Exception("Пользователь с таким именем уже существует");

        if (await _userRepository.ExistsByEmailAsync(input.Email))
            throw new Exception("Email уже зарегистрирован");
        
        if (input.RoleId.HasValue)
        {
            var role = await _roleRepository.GetByIdAsync(input.RoleId.Value);
            if (role == null)
                throw new Exception("Роль не найдена");
        }
        
        var user = new User
        {
            Username = input.Username,
            Email = input.Email,
            PasswordHash = PasswordHasher.Hash(input.Password),
            RoleId = input.RoleId ?? 2, 
            TeamId = input.TeamId == 0
                ? null
                : input.TeamId,
            CreatedAt = DateTime.UtcNow
        };

        var createdUser = await _userRepository.CreateAsync(user);

        return MapToResponse(createdUser);
    }
    

    public async Task<UserResponse> LoginAsync(string usernameOrEmail, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(usernameOrEmail)
            ?? await _userRepository.GetByEmailAsync(usernameOrEmail);

        if (user == null)
            throw new Exception("Неверный логин или пароль");
        
        if (!PasswordHasher.Verify(password, user.PasswordHash))
            throw new Exception("Неверный логин или пароль");
        
        await _userRepository.UpdateLastLoginAsync(user.Id);

        return MapToResponse(user);
    }

    public async Task LogoutAsync(int userId)
    {
        await Task.CompletedTask;
    }
    
    private UserResponse MapToResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.Username,
            user.Email,
            user.RoleId,
            user.Role?.Name,
            user.TeamId,
            user.Team?.Name,
            user.CreatedAt,
            user.LastLoginAt
        );
    }
}