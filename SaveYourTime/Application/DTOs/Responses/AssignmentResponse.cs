namespace WebApplication1.Application.DTOs.Responses;

public record AssignmentResponse(
    int Id,
    string Title,
    string? Description,
    int UserId,
    string UserName,
    int AssignmentStatusId,
    string StatusName,
    int? TeamId,
    string? TeamName,
    DateTime? DueDate,
    DateTime CreatedAt
);