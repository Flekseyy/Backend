using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Interfaces.Repositories;
using WebApplication1.Domain.Models;
using WebApplication1.Infrastructure.Contexts;

namespace WebApplication1.Infrastructure.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // RAZRABOTKA-18: Методы получения
    public async Task<IEnumerable<Assignment>> GetAllAsync()
    {
        return await _context.Assignments
            .Include(a => a.User)
            .Include(a => a.AssignmentStatus)
            .Include(a => a.AssignmentInfo)
            .ToListAsync();
    }

    public async Task<Assignment?> GetByIdAsync(int id)
    {
        return await _context.Assignments
            .Include(a => a.User)
            .Include(a => a.AssignmentStatus)
            .Include(a => a.AssignmentInfo)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Assignment>> GetByFilterAsync(string? title, int? statusId, int? userId)
    {
        var query = _context.Assignments
            .Include(a => a.User)
            .Include(a => a.AssignmentStatus)
            .Include(a => a.AssignmentInfo)
            .AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(a => a.AssignmentInfo.Name.Contains(title));
        }

        if (statusId.HasValue)
        {
            query = query.Where(a => a.AssignmentStatusId == statusId.Value);
        }

        if (userId.HasValue)
        {
            query = query.Where(a => a.UserId == userId.Value);
        }

        return await query.ToListAsync();
    }

    // RAZRABOTKA-19: Методы изменения
    public async Task<Assignment> CreateAsync(Assignment assignment)
    {
        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();
        return assignment;
    }

    public async Task UpdateAsync(Assignment assignment)
    {
        _context.Assignments.Update(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var assignment = await _context.Assignments.FindAsync(id);
        if (assignment != null)
        {
            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateStatusAsync(int assignmentId, int statusId)
    {
        var assignment = await _context.Assignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            assignment.AssignmentStatusId = statusId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task ChangeOwnerAsync(int assignmentId, int newUserId)
    {
        var assignment = await _context.Assignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            assignment.UserId = newUserId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateContentAsync(int assignmentId, string title, string? description)
    {
        var assignment = await _context.Assignments.FindAsync(assignmentId);
        if (assignment != null)
        {
            var info = await _context.AssignmentInfos.FindAsync(assignment.AssignmentInfoId);
            if (info != null)
            {
                info.Name = title;
                info.Description = description;
                await _context.SaveChangesAsync();
            }
        }
    }
}