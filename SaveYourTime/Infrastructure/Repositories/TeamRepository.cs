using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Interfaces.Repositories;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.Contexts;

namespace WebApplication1.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;

    public TeamRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<Team>> GetAllAsync() =>
        await _context.Teams.Include(t => t.Members).ToListAsync();

    public async Task<Team?> GetByIdAsync(int id) =>
        await _context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Team> CreateAsync(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team;
    }

    public async Task UpdateAsync(Team team)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var team = await _context.Teams.FindAsync(id);
        if (team != null)
        {
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }
    }
}