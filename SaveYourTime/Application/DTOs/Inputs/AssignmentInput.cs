namespace WebApplication1.Application.DTOs.Inputs;

public record AssignmentInput(
    string Title,
    string? Description,
    int UserId,
    int AssignmentStatusId,
    int? TeamId,
    DateTime? DueDate
);