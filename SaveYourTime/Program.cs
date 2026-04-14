using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Interface;
using WebApplication1.Infrastructure.Contexts;
using WebApplication1.Infrastructure.Middlewares;
using WebApplication1.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionStrings"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();