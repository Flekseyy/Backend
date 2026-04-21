using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Repositories;
using WebApplication1.Domain.Interfaces.Services;
using WebApplication1.Domain.Models;

namespace WebApplication1.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAssignmentStatusRepository _statusRepository;
    private readonly ITeamRepository _teamRepository;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        IUserRepository userRepository,
        IAssignmentStatusRepository statusRepository,
        ITeamRepository teamRepository)
    {
        _assignmentRepository = assignmentRepository;
        _userRepository = userRepository;
        _statusRepository = statusRepository;
        _teamRepository = teamRepository;
    }
    
    public async Task<IEnumerable<AssignmentResponse>> GetAllAsync()
    {
        var assignments = await _assignmentRepository.GetAllAsync();
        return assignments.Select(MapToResponse);
    }

    public async Task<AssignmentResponse?> GetByIdAsync(int id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        return assignment == null ? null : MapToResponse(assignment);
    }

    public async Task<IEnumerable<AssignmentResponse>> GetByFilterAsync(string? title, int? statusId, int? userId)
    {
        var assignments = await _assignmentRepository.GetByFilterAsync(title, statusId, userId);
        return assignments.Select(MapToResponse);
    }
    
    
    public async Task<AssignmentResponse> CreateAsync(AssignmentInput input)
    {
        var user = await _userRepository.GetByIdAsync(input.UserId);
        if (user == null)
            throw new Exception("Пользователь не найден");

        var status = await _statusRepository.GetByIdAsync(input.AssignmentStatusId);
        if (status == null)
            throw new Exception("Статус не найден");

        if (input.TeamId.HasValue)
        {
            var team = await _teamRepository.GetByIdAsync(input.TeamId.Value);
            if (team == null)
                throw new Exception("Команда не найдена");
        }
        
        var assignmentInfo = new AssignmentInfo
        {
            Name = input.Title,
            Description = input.Description
        };

        var assignment = new Assignment
        {
            AssignmentInfo =  assignmentInfo,
            UserId = input.UserId,
            AssignmentStatusId = input.AssignmentStatusId,
            AssignmentInfoId = 0,
            TeamId = input.TeamId,
            DueDate = input.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _assignmentRepository.CreateAsync(assignment);
        return MapToResponse(created);
    }

    public async Task<AssignmentResponse> UpdateAsync(int id, AssignmentInput input)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
            throw new Exception("Задача не найдена");

        assignment.UserId = input.UserId;
        assignment.AssignmentStatusId = input.AssignmentStatusId;
        assignment.TeamId = input.TeamId;
        assignment.DueDate = input.DueDate;

        await _assignmentRepository.UpdateAsync(assignment);
        return MapToResponse(assignment);
    }

    public async Task DeleteAsync(int id)
    {
        await _assignmentRepository.DeleteAsync(id);
    }

    public async Task<AssignmentResponse> UpdateStatusAsync(int assignmentId, int statusId)
    {
        var status = await _statusRepository.GetByIdAsync(statusId);
        if (status == null)
            throw new Exception("Статус не найден");

        await _assignmentRepository.UpdateStatusAsync(assignmentId, statusId);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        return MapToResponse(assignment!);
    }

    public async Task<AssignmentResponse> ChangeOwnerAsync(int assignmentId, int newUserId)
    {
        var user = await _userRepository.GetByIdAsync(newUserId);
        if (user == null)
            throw new Exception("Пользователь не найден");

        await _assignmentRepository.ChangeOwnerAsync(assignmentId, newUserId);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        return MapToResponse(assignment!);
    }

    public async Task<AssignmentResponse> UpdateContentAsync(int assignmentId, string title, string? description)
    {
        await _assignmentRepository.UpdateContentAsync(assignmentId, title, description);
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        return MapToResponse(assignment!);
    }

    // ========== Вспомогательные методы ==========
    
    private AssignmentResponse MapToResponse(Assignment assignment)
    {
        return new AssignmentResponse(
            assignment.Id,
            assignment.AssignmentInfo.Name,
            assignment.AssignmentInfo.Description,
            assignment.UserId,
            assignment.User.Username,
            assignment.AssignmentStatusId,
            assignment.AssignmentStatus.Name,
            assignment.TeamId,
            assignment.Team?.Name,
            assignment.DueDate,
            assignment.CreatedAt
        );
    }
}