using WebApplication1.Application.DTOs;

namespace WebApplication1.Application.Interfaces;

public interface IAuthService
{
    Task<UserResponse> RegisterAsync(UserInput input);
    Task<UserResponse> LoginAsync(string usernameOrEmail, string password);
    Task LogoutAsync(int userId);
}