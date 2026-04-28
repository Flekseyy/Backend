using WebApplication1.Application.Services;
using WebApplication1.Domain.Interfaces.Services;

namespace WebApplication1.Infrastructure.Middlewares;

public static class ServicesMiddlewares
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}