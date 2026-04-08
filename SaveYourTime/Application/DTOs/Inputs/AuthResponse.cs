namespace WebApplication1.Application.DTOs.Inputs;

public record AuthResponse(int UserId, string Username, string Email, DateTime ExpiresAt);