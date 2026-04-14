namespace WebApplication1.Domain.Models;

public class AssignmentInfo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}