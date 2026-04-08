namespace WebApplication1.Domain.Interface;
using WebApplication1.Domain.Object;
public interface IAssignmentRepository
{
    Task<IEnumerable<Assignment>> GetAllAsync();
    Task<Assignment?> GetByIdAsync(int id);
    Task<IEnumerable<Assignment>> GetByUserIdAsync(int userId);
    Task<Assignment> CreateAsync(Assignment assignment);
    Task UpdateAsync(Assignment assignment);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int assignmentId, int statusId);
}