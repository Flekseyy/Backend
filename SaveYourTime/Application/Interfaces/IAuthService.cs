using WebApplication1.Application.DTOs;

namespace WebApplication1.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterInput input); 
    Task<AuthResponse> LoginAsync(LoginInput input);       
}