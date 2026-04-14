using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Interfaces.Repositories;

public interface IAssignmentRepository
{
    Task<IEnumerable<Assignment>> GetAllAsync();
    Task<Assignment?> GetByIdAsync(int id);
    Task<IEnumerable<Assignment>> GetByFilterAsync(string? title, int? statusId, int? userId);
    
    Task<Assignment> CreateAsync(Assignment assignment);
    Task UpdateAsync(Assignment assignment);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int assignmentId, int statusId);
    Task ChangeOwnerAsync(int assignmentId, int newUserId);
    Task UpdateContentAsync(int assignmentId, string title, string? description);
}