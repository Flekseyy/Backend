using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;

namespace WebApplication1.Domain.Interfaces.Services;

public interface IUserService
{
    Task<UserResponse?> GetByIdAsync(int id);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<IEnumerable<UserResponse>> GetByTeamIdAsync(int teamId);
    Task<IEnumerable<UserResponse>> GetByFilterAsync(string? username, int? roleId);
    
    Task<UserResponse> CreateAsync(UserInput input);
    Task<UserResponse> UpdateAsync(int id, UserInput input);
    Task DeleteAsync(int id);
    Task<UserResponse> ChangeRoleAsync(int userId, int? roleId);
    Task<UserResponse> AddToTeamAsync(int userId, int teamId);
    Task<UserResponse> RemoveFromTeamAsync(int userId);
}