using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;

namespace WebApplication1.Domain.Interfaces.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync();
    Task<RoleResponse?> GetByIdAsync(int id);
    Task<IEnumerable<RoleResponse>> GetByFilterAsync(string? name);
    
    Task<RoleResponse> CreateAsync(RoleInput input);
    Task<RoleResponse> UpdateAsync(int id, RoleInput input);
    Task DeleteAsync(int id);
}