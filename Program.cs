using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MusicStreaming.Api.Data;
using MusicStreaming.Api.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Force URLs
builder.WebHost.UseUrls("http://localhost:5000");

// Database Configuration
var host = Environment.GetEnvironmentVariable("DB_HOST");
var port = Environment.GetEnvironmentVariable("DB_PORT");
var database = Environment.GetEnvironmentVariable("DB_NAME");
var username = Environment.GetEnvironmentVariable("DB_USERNAME");
var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

builder.Configuration["Jwt:Key"] = Environment.GetEnvironmentVariable("JWT_KEY");
builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER");
builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

builder.Services.AddScoped<AuthFactory>();

builder.Services.AddScoped<EmailAuthProvider>();
builder.Services.AddScoped<GoogleAuthProvider>();

builder.Services.AddScoped<JwtService>();

var connectionString =
    $"Host={host};Port={port};Database={database};Username={username};Password={password}";

// Services
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Nice startup log
Console.WriteLine("\n=================================");
Console.WriteLine("Music Streaming API Started");
Console.WriteLine("=================================");
Console.WriteLine("Backend: http://localhost:5000");
Console.WriteLine("Swagger: http://localhost:5000/swagger");
Console.WriteLine("=================================\n");

app.Run();