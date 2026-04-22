namespace WebApplication1.Application.DTOs.Inputs;

public record TeamInput(
    string Name,
    string? Description = null,
    int? LeaderId = null
);