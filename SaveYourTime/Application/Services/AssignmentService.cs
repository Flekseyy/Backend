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
    private readonly IAssignmentPriorityRepository _priorityRepository;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        IUserRepository userRepository,
        IAssignmentPriorityRepository priorityRepository)
    {
        _assignmentRepository = assignmentRepository;
        _userRepository = userRepository;
        _priorityRepository = priorityRepository;
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

    public async Task<AssignmentResponse> CreateAsync(AssignmentInput input, int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new Exception("Пользователь не найден");

        int priorityId = input.Priority switch
        {
            "low" => 1,
            "medium" => 2,
            "high" => 3,
            _ => 2
        };

        var assignment = new Assignment
        {
            Title = input.Title,
            Description = input.Description,
            UserId = userId,
            StatusId = 1,
            PriorityId = priorityId,
            TeamId = input.TeamId,
            Deadline = input.Deadline,
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

        assignment.Title = input.Title;
        assignment.Description = input.Description;
        assignment.PriorityId = input.Priority switch
        {
            "low" => 1,
            "medium" => 2,
            "high" => 3,
            _ => 2
        };
        assignment.Deadline = input.Deadline;

        await _assignmentRepository.UpdateAsync(assignment);
        return MapToResponse(assignment);
    }

    public async Task DeleteAsync(int id)
    {
        await _assignmentRepository.DeleteAsync(id);
    }

    public async Task<AssignmentResponse> UpdateStatusAsync(int id, string status)
    {
        int statusId = status switch
        {
            "todo" => 1,
            "in-progress" => 2,
            "done" => 3,
            _ => 1
        };

        await _assignmentRepository.UpdateStatusAsync(id, statusId);
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        return MapToResponse(assignment!);
    }

    public async Task<AssignmentResponse> ChangeOwnerAsync(int assignmentId, int newUserId)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment == null)
            throw new Exception("Задача не найдена");

        var user = await _userRepository.GetByIdAsync(newUserId);
        if (user == null)
            throw new Exception("Пользователь не найден");

        await _assignmentRepository.ChangeOwnerAsync(assignmentId, newUserId);
        
        var updated = await _assignmentRepository.GetByIdAsync(assignmentId);
        return MapToResponse(updated!);
    }

    public async Task<AssignmentResponse> UpdateContentAsync(int assignmentId, string title, string? description)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment == null)
            throw new Exception("Задача не найдена");

        await _assignmentRepository.UpdateContentAsync(assignmentId, title, description);
        
        var updated = await _assignmentRepository.GetByIdAsync(assignmentId);
        return MapToResponse(updated!);
    }

    private AssignmentResponse MapToResponse(Assignment a)
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