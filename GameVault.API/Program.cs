using GameVault.API.Data;
using GameVault.API.Mapping;
using GameVault.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container

// Register the GameVaultContext with SQL Server
builder.Services.AddDbContext<GameVaultContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

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

// Keep HTTP available in local development so mobile clients can reach the API over LAN.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Map controllers
app.MapControllers();

app.Run();
