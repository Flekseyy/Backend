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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<AssignmentStatus>().HasData(
            new AssignmentStatus { Id = 1, Name = "To Do" },
            new AssignmentStatus { Id = 2, Name = "In Progress" },
            new AssignmentStatus { Id = 3, Name = "Done" }
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
    }
}