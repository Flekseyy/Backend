using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Interfaces.Repositories;

public interface ITeamRepository
{
    Task<IEnumerable<Team>> GetAllAsync();
    Task<Team?> GetByIdAsync(int id);
    Task<Team> CreateAsync(Team team);
    Task UpdateAsync(Team team);
    Task DeleteAsync(int id);
}