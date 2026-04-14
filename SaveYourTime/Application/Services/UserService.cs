using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Repositories;
using WebApplication1.Domain.Interfaces.Services;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.Utils;

namespace WebApplication1.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITeamRepository _teamRepository;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ITeamRepository teamRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _teamRepository = teamRepository;
    }

    
    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToResponse);
    }

    public async Task<IEnumerable<UserResponse>> GetByTeamIdAsync(int teamId)
    {
        var users = await _userRepository.GetByTeamIdAsync(teamId);
        return users.Select(MapToResponse);
    }

    public async Task<IEnumerable<UserResponse>> GetByFilterAsync(string? username, int? roleId)
    {
        var users = await _userRepository.GetByFilterAsync(username, roleId);
        return users.Select(MapToResponse);
    }
    
    public async Task<UserResponse> CreateAsync(UserInput input)
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

        if (input.TeamId.HasValue)
        {
            var team = await _teamRepository.GetByIdAsync(input.TeamId.Value);
            if (team == null)
                throw new Exception("Команда не найдена");
        }

        var user = new User
        {
            Username = input.Username,
            Email = input.Email,
            PasswordHash = PasswordHasher.Hash(input.Password),
            RoleId = input.RoleId,
            TeamId = input.TeamId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.CreateAsync(user);
        return MapToResponse(created);
    }

    public async Task<UserResponse> UpdateAsync(int id, UserInput input)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("Пользователь не найден");

        user.Username = input.Username;
        user.Email = input.Email;
        user.RoleId = input.RoleId;
        user.TeamId = input.TeamId;

        await _userRepository.UpdateAsync(user);
        return MapToResponse(user);
    }

    public async Task DeleteAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task<UserResponse> ChangeRoleAsync(int userId, int? roleId)
    {
        if (roleId.HasValue)
        {
            var role = await _roleRepository.GetByIdAsync(roleId.Value);
            if (role == null)
                throw new Exception("Роль не найдена");
        }

        await _userRepository.ChangeRoleAsync(userId, roleId);
        var user = await _userRepository.GetByIdAsync(userId);
        return MapToResponse(user!);
    }

    public async Task<UserResponse> AddToTeamAsync(int userId, int teamId)
    {
        var team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            throw new Exception("Команда не найдена");

        await _userRepository.AddToTeamAsync(userId, teamId);
        var user = await _userRepository.GetByIdAsync(userId);
        return MapToResponse(user!);
    }

    public async Task<UserResponse> RemoveFromTeamAsync(int userId)
    {
        await _userRepository.RemoveFromTeamAsync(userId);
        var user = await _userRepository.GetByIdAsync(userId);
        return MapToResponse(user!);
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