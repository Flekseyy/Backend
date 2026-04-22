using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;

namespace WebApplication1.Domain.Interfaces.Services;

public interface IAssignmentService
{
    Task<IEnumerable<AssignmentResponse>> GetAllAsync();
    Task<AssignmentResponse?> GetByIdAsync(int id);
    Task<IEnumerable<AssignmentResponse>> GetByFilterAsync(string? title, int? statusId, int? userId);
    
    Task<AssignmentResponse> CreateAsync(AssignmentInput input, int userId);
    Task<AssignmentResponse> UpdateAsync(int id, AssignmentInput input);
    Task DeleteAsync(int id);
    Task<AssignmentResponse> UpdateStatusAsync(int assignmentId, string status);
    Task<AssignmentResponse> ChangeOwnerAsync(int assignmentId, int newUserId);
    Task<AssignmentResponse> UpdateContentAsync(int assignmentId, string title, string? description);
}