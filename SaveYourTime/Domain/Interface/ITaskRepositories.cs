namespace WebApplication1.Domain.Interface;

public interface ITaskRepositories
{
    Task<Task?> GetByUsername(string username);
    Task<Task?> GetById(string id);
    Task<IEnumerable<Task>> GetAll();
    Task<Task> Create(Task task);
    Task<Task> Update(Task task);
    Task Delete(string id);
}