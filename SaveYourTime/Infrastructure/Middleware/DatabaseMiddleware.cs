using WebApplication1.Infrastructure.Data;
namespace WebApplication1.Infrastructure.Middleware;

public static class DatabaseMiddleware
{
    public static WebApplication UseDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Database initialization error");
            throw;
        }
        
        return app;
    }
}