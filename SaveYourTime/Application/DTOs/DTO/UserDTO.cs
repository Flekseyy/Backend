namespace WebApplication1.Application.DTOs.DTO;

public record UserDTO(
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