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
        await _context.Teams
            .Include(t => t.Leader)
            .Include(t => t.Members)
            .Include(t => t.Assignments)
            .ToListAsync();

    public async Task<Team?> GetByIdAsync(int id) =>
         await _context.Teams
            .Include(t => t.Leader)
            .Include(t => t.Members)
            .Include(t => t.Assignments)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<User>> GetUsersInTeamAsync(int teamId)
    {
        var team = await _context.Teams
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == teamId);

        return team?.Members ?? Enumerable.Empty<User>();
    }

    public async Task<IEnumerable<Assignment>> GetAssignmentsInTeamAsync(int teamId) =>
        await _context.Assignments
            .Include(a => a.User)
            .Include(a => a.Status)
            .Include(a => a.Priority)
            .Where(a => a.TeamId == teamId)
            .ToListAsync();

    public async Task<Team> CreateAsync(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team;
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

    public async Task UpdateAsync(Team team)
    {
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserToTeamAsync(int userId, int teamId)
    {
        var user = await _context.Users
            .Include(u => u.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
        {
            throw new Exception("Team not found");
        }

        bool alreadyInTeam = user.Teams.Any(t => t.Id == teamId);

        if (!alreadyInTeam)
        {
            user.Teams.Add(team);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveUserFromTeamAsync(int userId, int teamId)
    {
        var user = await _context.Users
            .Include(u => u.Teams)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        var team = await _context.Teams
            .FirstOrDefaultAsync(t => t.Id == teamId);

        if (team == null)
        {
            throw new Exception("Team not found");
        }

        bool alreadyInTeam = user.Teams.Any(t => t.Id == teamId);

        if (alreadyInTeam)
        {
            user.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SetTeamLeaderAsync(int teamId, int userId)
    {
        var team = await _context.Teams.FindAsync(teamId);
        if (team != null)
        {
            team.LeaderId = userId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task ChangeTeamLeaderAsync(int teamId, int newLeaderId)
    {
        var team = await _context.Teams.FindAsync(teamId);
        if (team != null)
        {
            team.LeaderId = newLeaderId;
            await _context.SaveChangesAsync();
        }
    }
}