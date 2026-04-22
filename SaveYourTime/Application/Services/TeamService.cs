using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Repositories;
using WebApplication1.Domain.Interfaces.Services;
using WebApplication1.Domain.Models;

namespace WebApplication1.Application.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAssignmentRepository _assignmentRepository;

    public TeamService(
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        IAssignmentRepository assignmentRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
        _assignmentRepository = assignmentRepository;
    }

    public async Task<IEnumerable<TeamResponse>> GetAllAsync()
    {
        var teams = await _teamRepository.GetAllAsync();
        return teams.Select(MapToResponse);
    }

    public async Task<TeamResponse?> GetByIdAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        return team == null ? null : MapToResponse(team);
    }

    public async Task<IEnumerable<UserResponse>> GetUsersInTeamAsync(int teamId)
    {
        var users = await _teamRepository.GetUsersInTeamAsync(teamId);
        return users.Select(u => new UserResponse(
            u.Id, u.Username, u.Email, u.RoleId, u.Role?.Name,
            u.TeamId, u.Team?.Name, u.CreatedAt, u.LastLoginAt
        ));
    }

    public async Task<IEnumerable<AssignmentResponse>> GetAssignmentsInTeamAsync(int teamId)
    {
        var assignments = await _teamRepository.GetAssignmentsInTeamAsync(teamId);
        return assignments.Select(MapAssignmentToResponse);
    }

    public async Task<TeamResponse> CreateAsync(TeamInput input)
    {
        if (input.LeaderId.HasValue)
        {
            var leader = await _userRepository.GetByIdAsync(input.LeaderId.Value);
            if (leader == null)
                throw new Exception("Лидер не найден");
        }

        var team = new Team
        {
            Name = input.Name,
            Description = input.Description,
            LeaderId = input.LeaderId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _teamRepository.CreateAsync(team);
        return MapToResponse(created);
    }

    public async Task<TeamResponse> UpdateAsync(int id, TeamInput input)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
            throw new Exception("Команда не найдена");

        team.Name = input.Name;
        team.Description = input.Description;
        team.LeaderId = input.LeaderId;

        await _teamRepository.UpdateAsync(team);
        return MapToResponse(team);
    }

    public async Task DeleteAsync(int id)
    {
        await _teamRepository.DeleteAsync(id);
    }

    public async Task<TeamResponse> AddUserToTeamAsync(int userId, int teamId)
    {
        await _teamRepository.AddUserToTeamAsync(userId, teamId);
        var team = await _teamRepository.GetByIdAsync(teamId);
        return MapToResponse(team!);
    }

    public async Task<TeamResponse> RemoveUserFromTeamAsync(int userId)
    {
        await _teamRepository.RemoveUserFromTeamAsync(userId);
        var user = await _userRepository.GetByIdAsync(userId);
        var team = user?.TeamId.HasValue == true
            ? await _teamRepository.GetByIdAsync(user.TeamId.Value)
            : null;
        return MapToResponse(team!);
    }

    public async Task<TeamResponse> SetTeamLeaderAsync(int teamId, int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("Пользователь не найден");

        await _teamRepository.SetTeamLeaderAsync(teamId, userId);
        var team = await _teamRepository.GetByIdAsync(teamId);
        return MapToResponse(team!);
    }

    public async Task<TeamResponse> ChangeTeamLeaderAsync(int teamId, int newLeaderId)
    {
        var user = await _userRepository.GetByIdAsync(newLeaderId);
        if (user == null)
            throw new Exception("Пользователь не найден");

        await _teamRepository.ChangeTeamLeaderAsync(teamId, newLeaderId);
        var team = await _teamRepository.GetByIdAsync(teamId);
        return MapToResponse(team!);
    }

    private TeamResponse MapToResponse(Team team)
    {
        return new TeamResponse(
            team.Id,
            team.Name,
            team.Description,
            team.LeaderId,
            team.Leader?.Username,
            team.CreatedAt
        );
    }

    private AssignmentResponse MapAssignmentToResponse(Assignment a)
    {
        return new AssignmentResponse(
            a.Id,
            a.Title,
            a.Description,
            a.UserId,
            a.User.Username,
            a.Status.Name,
            a.Priority.Name,
            a.TeamId,
            a.Team?.Name,
            a.Deadline,
            a.CreatedAt
        );
    }
}