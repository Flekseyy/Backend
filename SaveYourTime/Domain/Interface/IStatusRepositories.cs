using WebApplication1.Domain.Object;

namespace WebApplication1.Domain.Interface;

public interface IStatusRepository
{
    Task<IEnumerable<Status>> GetAllAsync();
    Task<Status?> GetByIdAsync(int id);
}