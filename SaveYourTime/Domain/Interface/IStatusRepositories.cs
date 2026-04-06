namespace WebApplication1.Domain.Interface;

public interface IStatusRepositories
{
    Task<IEnumerable<Status>> GetAll();
    Task<Status?> GetById(string id);
}