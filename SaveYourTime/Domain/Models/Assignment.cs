namespace WebApplication1.Domain.Models;

public class Assignment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int AssignmentStatusId { get; set; }
    public int AssignmentInfoId { get; set; }
    public int? TeamId { get; set; }          
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public User User { get; set; } = null!;
    public AssignmentStatus AssignmentStatus { get; set; } = null!;
    public AssignmentInfo AssignmentInfo { get; set; } = null!;
    public Team? Team { get; set; }          
}