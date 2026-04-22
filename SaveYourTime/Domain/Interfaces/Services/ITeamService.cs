using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;

namespace WebApplication1.Domain.Interfaces.Services;

public interface ITeamService
{
    Task<IEnumerable<TeamResponse>> GetAllAsync();
    Task<TeamResponse?> GetByIdAsync(int id);
    Task<IEnumerable<UserResponse>> GetUsersInTeamAsync(int teamId);
    Task<IEnumerable<AssignmentResponse>> GetAssignmentsInTeamAsync(int teamId);

    Task<TeamResponse> CreateAsync(TeamInput input);
    Task<TeamResponse> UpdateAsync(int id, TeamInput input);
    Task DeleteAsync(int id);
    Task<TeamResponse> AddUserToTeamAsync(int userId, int teamId);
    Task<TeamResponse> RemoveUserFromTeamAsync(int userId);
    Task<TeamResponse> SetTeamLeaderAsync(int teamId, int userId);
    Task<TeamResponse> ChangeTeamLeaderAsync(int teamId, int newLeaderId);
}