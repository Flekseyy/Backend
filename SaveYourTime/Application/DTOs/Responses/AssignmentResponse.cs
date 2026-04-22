namespace WebApplication1.Application.DTOs.Responses;

public record AssignmentResponse(
    int Id,
    string Title,
    string? Description,
    int UserId,
    string UserName,
    string Status,      
    string Priority,
    int? TeamId,
    string? TeamName,
    DateTime? Deadline,
    DateTime CreatedAt
);