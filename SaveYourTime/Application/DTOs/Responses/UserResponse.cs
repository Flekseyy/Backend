namespace WebApplication1.Application.DTOs.Responses;

public record UserResponse(
    int Id,
    string Username,
    string Email,
    int? RoleId,
    string? RoleName,
    int? TeamId,
    string? TeamName,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);