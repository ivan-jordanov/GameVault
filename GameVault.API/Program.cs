using GameVault.API.Data;
using GameVault.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container

// Register the GameVaultContext with SQL Server
builder.Services.AddDbContext<GameVaultContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register service layer for dependency injection
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<NewsService>();
builder.Services.AddScoped<WebResourceService>();

// Add controllers
builder.Services.AddControllers();

// Add CORS policy that allows any origin, any method, any header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Swagger/OpenAPI for development
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline

// Enable CORS
app.UseCors("AllowAll");

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Use HTTPS redirection
app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.Run();
