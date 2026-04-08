namespace WebApplication1.Application.DTOs;

public record AuthResponse(
    int UserId,
    string Username,
    string Email,
    DateTime ExpiresAt,
    string? RoleName
    );