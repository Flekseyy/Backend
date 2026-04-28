using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Models;

namespace WebApplication1.Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentStatus> AssignmentStatuses => Set<AssignmentStatus>();
    public DbSet<AssignmentInfo> AssignmentInfos => Set<AssignmentInfo>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<AssignmentStatus>().HasData(
            new AssignmentStatus { Id = 1, Name = "To Do" },
            new AssignmentStatus { Id = 2, Name = "In Progress" },
            new AssignmentStatus { Id = 3, Name = "Done" }
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 0, Name = "Admin", Description = "Administrator" },
            new Role { Id = 1, Name = "User", Description = "Regular user" },
            new Role { Id = 2, Name = "TeamLead", Description = "Team Leader" }
        );
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Assignments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.AssignmentStatus)
            .WithMany(s => s.Assignments)
            .HasForeignKey(a => a.AssignmentStatusId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.AssignmentInfo)
            .WithMany(i => i.Assignments)
            .HasForeignKey(a => a.AssignmentInfoId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<User>()
            .HasOne(u => u.Team)
            .WithMany(t => t.Members)
            .HasForeignKey(u => u.TeamId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Team>()
            .HasMany(t => t.Assignments)
            .WithOne(a => a.Team)
            .HasForeignKey(a => a.TeamId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Team>()
            .HasOne(t => t.Leader)
            .WithMany()
            .HasForeignKey(t => t.LeaderId)
            .OnDelete(DeleteBehavior.SetNull); 
    }
}