using WebApplication1.Domain.Models;

namespace WebApplication1.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetByTeamIdAsync(int teamId);
    Task<IEnumerable<User>> GetByFilterAsync(string? username, int? roleId);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task ChangeRoleAsync(int userId, int? roleId);
    Task AddToTeamAsync(int userId, int teamId);
    Task RemoveFromTeamAsync(int userId);
    
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);
    Task UpdateLastLoginAsync(int userId);
}