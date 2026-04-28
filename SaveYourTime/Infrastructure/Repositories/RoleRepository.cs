using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Interfaces.Repositories;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.Contexts;

namespace WebApplication1.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Role>> GetAllAsync() =>
        await _context.Roles.ToListAsync();

    public async Task<Role?> GetByIdAsync(int id) =>
        await _context.Roles.FindAsync(id);

    public async Task<Role> CreateAsync(Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task UpdateAsync(Role role)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        if (role != null)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}