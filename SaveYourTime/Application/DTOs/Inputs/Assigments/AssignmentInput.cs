namespace WebApplication1.Application.DTOs.Inputs.Assigments;

public record AssignmentInput(
    int UserId,
    string Title,
    string? Description,
    string Priority,
    DateTime? Deadline
);