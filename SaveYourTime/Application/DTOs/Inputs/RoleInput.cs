namespace WebApplication1.Application.DTOs.Inputs;

public record RoleInput(
    string Name,
    string? Description = null
);