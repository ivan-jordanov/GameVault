using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Services;

public class GameService
{
    private readonly GameVaultContext _context;

    public GameService(GameVaultContext context)
    {
        _context = context;
    }

    // Get all games with sorting
    public async Task<List<GameSummaryDto>> GetAllGamesAsync(string? sort = null)
    {
        var query = GetGameSummaryQuery();

        // Apply sorting
        query = sort?.ToLower() switch
        {
            "rating" => query.OrderByDescending(g => g.AverageRating ?? 0),
            "releasedate" => query.OrderByDescending(g => g.ReleaseDate),
            "alphabetical" => query.OrderBy(g => g.Title),
            _ => query.OrderByDescending(g => g.AverageRating ?? 0) // Default to rating
        };

        return await query.ToListAsync();
    }

    // Search games by title, category, and platform
    public async Task<List<GameSummaryDto>> SearchGamesAsync(string? q, int? categoryId = null, int? platformId = null)
    {
        var query = GetGameSummaryQuery();

        // Filter by title (case-insensitive)
        if (!string.IsNullOrWhiteSpace(q))
        {
            var searchTerm = q.ToLower();
            query = query.Where(g => g.Title.ToLower().Contains(searchTerm));
        }

        // Filter by category if provided
        if (categoryId.HasValue)
        {
            query = query.Where(g => g.Categories.Any(c => c.CategoryId == categoryId.Value));
        }

        // Filter by platform if provided
        if (platformId.HasValue)
        {
            query = query.Where(g => g.Platforms.Any(p => p.PlatformId == platformId.Value));
        }

        // Default sort by rating
        query = query.OrderByDescending(g => g.AverageRating ?? 0);

        return await query.ToListAsync();
    }

    // Get a single game with full details
    public async Task<GameDetailDto?> GetGameByIdAsync(int id)
    {
        return await _context.Games
            .Where(g => g.GameId == id)
            .Select(g => new GameDetailDto
            {
                GameId = g.GameId,
                Title = g.Title,
                Description = g.Description,
                CoverArtUrl = g.CoverArtUrl,
                Developer = g.Developer,
                Publisher = g.Publisher,
                ReleaseDate = g.ReleaseDate,
                Images = g.GameImages
                    .OrderBy(gi => gi.DisplayOrder)
                    .Select(gi => new GameImageDto
                    {
                        ImageUrl = gi.ImageUrl,
                        DisplayOrder = gi.DisplayOrder
                    })
                    .ToList(),
                Categories = g.Categories.Select(c => c.Name).ToList(),
                Platforms = g.Platforms.Select(p => p.Name).ToList(),
                ReviewCount = g.Reviews.Count(),
                AverageRating = g.Reviews.Count() >= 5
                    ? (double?)g.Reviews.Average(r => r.Rating)
                    : null
            })
            .FirstOrDefaultAsync();
    }

    // Helper method to build the game summary query with all necessary projections
    private IQueryable<GameSummaryDto> GetGameSummaryQuery()
    {
        return _context.Games
            .Select(g => new GameSummaryDto
            {
                GameId = g.GameId,
                Title = g.Title,
                CoverArtUrl = g.CoverArtUrl,
                Developer = g.Developer,
                Publisher = g.Publisher,
                ReleaseDate = g.ReleaseDate,
                Categories = g.Categories.Select(c => c.Name).ToList(),
                Platforms = g.Platforms.Select(p => p.Name).ToList(),
                ReviewCount = g.Reviews.Count(),
                AverageRating = g.Reviews.Count() >= 5
                    ? (double?)g.Reviews.Average(r => r.Rating)
                    : null
            });
    }
}
