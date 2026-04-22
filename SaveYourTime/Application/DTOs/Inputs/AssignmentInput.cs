namespace WebApplication1.Application.DTOs.Inputs;

public record AssignmentInput(
    string Title,
    string? Description,
    string Priority,
    DateTime? Deadline,
    int? TeamId = null
);