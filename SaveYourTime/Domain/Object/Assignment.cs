using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace WebApplication1.Domain.Object;

public class Assignment
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    public User Owner { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public Team? Team { get; set; }
    
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int StatusId { get; set; }
    public int? TeamId { get; set; }
    
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}