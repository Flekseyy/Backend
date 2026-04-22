namespace WebApplication1.Application.DTOs.Inputs;

public record UserInput(
    string Username,
    string Email,
    string Password,
    int? RoleId = 2,
    int? TeamId = null
);