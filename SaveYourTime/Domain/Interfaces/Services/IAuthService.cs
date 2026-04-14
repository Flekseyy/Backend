using WebApplication1.Application.DTOs;
using WebApplication1.Application.DTOs.Responses;

namespace WebApplication1.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterInput input);
}