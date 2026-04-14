namespace WebApplication1.Application.DTOs.Responses;

public record LoginInput(
    string UsernameOrEmail,
    string Password
);
