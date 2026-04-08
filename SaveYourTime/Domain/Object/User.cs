namespace WebApplication1.Domain.Object;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int? RoleId { get; set; }
    public int? TeamId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    public Role? Role { get; set; }
    public Team? Team { get; set; }
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}