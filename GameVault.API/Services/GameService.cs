using AutoMapper;
using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameVault.API.Services;

public class GameService
{
    private readonly GameVaultContext _context;
    private readonly IMapper _mapper;

    public GameService(GameVaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Get all games with sorting
    public async Task<List<GameSummaryDto>> GetAllGamesAsync(string? sort = null)
    {
        var games = await _context.Games
            .Include(g => g.Categories)
            .Include(g => g.Platforms)
            .Include(g => g.Reviews)
            .AsNoTracking()
            .ToListAsync();

        var dtos = _mapper.Map<List<GameSummaryDto>>(games);

        // Apply sorting
        dtos = sort?.ToLower() switch
        {
            "rating" => dtos.OrderByDescending(g => g.AverageRating ?? 0).ToList(),
            "releasedate" => dtos.OrderByDescending(g => g.ReleaseDate).ToList(),
            "alphabetical" => dtos.OrderBy(g => g.Title).ToList(),
            _ => dtos.OrderByDescending(g => g.AverageRating ?? 0).ToList() // Default to rating
        };

        return dtos;
    }

    // Search games by title, category, and platform
    public async Task<List<GameSummaryDto>> SearchGamesAsync(string? q, int? categoryId = null, int? platformId = null)
    {
        // Start with the Games DbSet to filter on entities
        var gameQuery = _context.Games
            .Include(g => g.Categories)
            .Include(g => g.Platforms)
            .Include(g => g.Reviews)
            .AsNoTracking()
            .AsQueryable();

        // Filter by title (case-insensitive)
        if (!string.IsNullOrWhiteSpace(q))
        {
            var searchTerm = q.ToLower();
            gameQuery = gameQuery.Where(g => g.Title.ToLower().Contains(searchTerm));
        }

        // Filter by category if provided
        if (categoryId.HasValue)
        {
            gameQuery = gameQuery.Where(g => g.Categories.Any(c => c.CategoryId == categoryId.Value));
        }

        // Filter by platform if provided
        if (platformId.HasValue)
        {
            gameQuery = gameQuery.Where(g => g.Platforms.Any(p => p.PlatformId == platformId.Value));
        }

        var games = await gameQuery.ToListAsync();
        var dtos = _mapper.Map<List<GameSummaryDto>>(games);

        // Sort by rating (default)
        dtos = dtos.OrderByDescending(g => g.AverageRating ?? 0).ToList();

        return dtos;
    }

    // Get a single game with full details
    public async Task<GameDetailDto?> GetGameByIdAsync(int id)
    {
        var game = await _context.Games
            .Include(g => g.Categories)
            .Include(g => g.Platforms)
            .Include(g => g.Reviews)
            .Include(g => g.GameImages)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.GameId == id);

        if (game == null)
            return null;

        return _mapper.Map<GameDetailDto>(game);
    }
}
