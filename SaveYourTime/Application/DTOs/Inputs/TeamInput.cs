namespace WebApplication1.Application.DTOs.Inputs;

public record TeamInput(
    string Name,
    int LeaderId,
    string? Description = null
);