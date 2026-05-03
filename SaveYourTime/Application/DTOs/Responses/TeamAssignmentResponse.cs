namespace WebApplication1.Application.DTOs.Responses;

public record TeamAssignmentResponse(
    int Id,
    string Name,
    string? Description,
    int TeamId,
    int StatusId,
    string Status,
    int? UserId,
    string? UserName,
    DateTime CreatedAt
);

