namespace WebApplication1.Infrastructure.Data;
using WebApplication1.Domain.Object;
using Microsoft.EntityFrameworkCore;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Status>().HasData(
            new Status { Id = 1, Name = "To Do" },
            new Status { Id = 2, Name = "In Progress" },
            new Status { Id = 3, Name = "Done" }
        );
        
        modelBuilder.Entity<Role>().HasData(
            
            new Role { Id = 0, Name = "Admin", Description = "Administrator" },
            new Role { Id = 1, Name = "Leader", Description = "Team Leader" },
            new Role { Id = 2, Name = "User", Description = "Regular user" }
        );
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Owner)
            .WithMany(u => u.Assignments)
            .HasForeignKey(a => a.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Status)
            .WithMany(s => s.Assignments)
            .HasForeignKey(a => a.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

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
    }
}