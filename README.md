# GameVault API

A read-only .NET 8 Web API for the GameVault game tracking platform. This API serves guest-facing endpoints with no authentication required.

## Features

- **Games Management**: Browse games with filtering and sorting
- **Reviews**: Read user reviews with sorting options
- **Categories & Platforms**: Browse available game categories and platforms
- **News**: View published game news
- **Web Resources**: Access website resources

## Project Structure

```
GameVault.API/
├── Controllers/           # API endpoint handlers
├── Models/               # Entity models (Game, User, Review, etc.)
├── DTOs/                 # Data Transfer Objects
├── Data/                 # EF Core DbContext
├── Services/             # Business logic layer
├── Program.cs            # Application configuration
├── appsettings.json      # Configuration with connection string
└── GameVault.API.csproj  # Project file
```

## Prerequisites

- .NET 8 SDK
- SQL Server (with GameVault database and seed data already created)

## Setup

1. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

2. **Verify the database connection string** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": ""
   }
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the API**
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` and Swagger documentation at `https://localhost:5001/swagger`.

## API Endpoints

### Games
- `GET /api/games` - Get all games (sort: "rating", "releasedate", "alphabetical")
- `GET /api/games/{id}` - Get game details
- `GET /api/games/search?q=title&categoryId=1&platformId=2` - Search games

### Reviews
- `GET /api/reviews/game/{gameId}` - Get reviews for a game (sort: "newest", "oldest", "highest", "lowest")

### Categories & Platforms
- `GET /api/categories` - Get all categories
- `GET /api/platforms` - Get all platforms

### News
- `GET /api/news` - Get published news

### Web Resources
- `GET /api/webresources` - Get all web resources
- `GET /api/webresources/{id}` - Get a specific web resource

## Key Design Points

- **Read-only API**: No POST, PUT, DELETE operations
- **No Authentication**: All endpoints are public
- **Service Layer**: All database access goes through services
- **Async/Await**: All database operations are async
- **Direct DTO Projection**: Services use IQueryable and .Select() to project to DTOs
- **CORS Enabled**: Allows requests from any origin
- **EF Core 8**: Uses Fluent API for entity configuration
