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

    public async Task RegisterAsync(UserInput input)
    {
        if (await _userRepository.ExistsByEmailAsync(input.Email))
            throw new Exception("Email уже зарегистрирован");

        var roleId = await _roleRepository.GetDefaultRoleId();

        var user = new User
        {
            Username = input.Username,
            Email = input.Email,
            PasswordHash = PasswordHasher.Hash(input.Password),
            RoleId = roleId,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);
    }

    public async Task<UserResponse> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null || !PasswordHasher.Verify(password, user.PasswordHash))
            throw new Exception("Неверный email или пароль");

        await _userRepository.UpdateLastLoginAsync(user.Id);
        return MapToResponse(user);
    }

    private UserResponse MapToResponse(User user) =>
        new UserResponse(
            user.Id,
            user.Username,
            user.Email,
            user.RoleId,
            user.Role?.Name,
            user.CreatedAt,
            user.LastLoginAt
        );
}