namespace WebApplication1.Domain.Interface;

public interface IUserRepositories
{
    Task<User?> GetByUsername(string username);
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(string id);
    Task<IEnumerable<User>> GetAll();
    Task<User> Create(User user);
    Task<User> Update(User user);
    Task Delete(string id);
}