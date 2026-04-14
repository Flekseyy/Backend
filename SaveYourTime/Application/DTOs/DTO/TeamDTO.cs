namespace WebApplication1.Application.DTOs.DTO;

public record TeamDTO(
    int Id,
    string Name,
    string? Description,
    int? LeaderId,
    string? LeaderName,
    DateTime CreatedAt
);