using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Application.DTOs.Inputs;
using WebApplication1.Application.DTOs.Responses;
using WebApplication1.Domain.Interfaces.Services;

namespace WebApplication1.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] UserInput input)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(input.Username) || input.Username.Length < 3)
                return BadRequest("Имя пользователя должно содержать минимум 3 символа");

            if (string.IsNullOrWhiteSpace(input.Email) || !IsValidEmail(input.Email))
                return BadRequest("Некорректный email");

            if (string.IsNullOrWhiteSpace(input.Password) || input.Password.Length < 6)
                return BadRequest("Пароль должен содержать минимум 6 символов");
            
            var response = await _authService.RegisterAsync(input);
      
            await SetAuthCookie(response.Id, response.Username, response.Email, DateTime.UtcNow.AddDays(7));
            
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginInput input)
    {
        try
        {
            var response = await _authService.LoginAsync(input.UsernameOrEmail, input.Password);

            await SetAuthCookie(response.Id, response.Username, response.Email, DateTime.UtcNow.AddDays(7));

            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Выход выполнен успешно" });
    }
    
    [HttpGet("me")]
    public ActionResult<UserResponse> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        return Ok(new { userId = int.Parse(userId), username = User.Identity?.Name });
    }
    
    private async Task SetAuthCookie(int userId, string username, string email, DateTime expiresAt)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties 
            { 
                ExpiresUtc = expiresAt, 
                IsPersistent = true 
            });
    }

    private bool IsValidEmail(string email)
    {
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
    }
}