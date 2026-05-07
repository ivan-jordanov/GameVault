using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Services;

public class ReviewService
{
    private readonly GameVaultContext _context;

    public ReviewService(GameVaultContext context)
    {
        _context = context;
    }

    // Get all reviews for a specific game with sorting
    public async Task<List<ReviewDto>> GetReviewsByGameIdAsync(int gameId, string? sort = null)
    {
        var query = _context.Reviews
            .Where(r => r.GameId == gameId)
            .Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                GameId = r.GameId,
                GameTitle = r.Game.Title,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                CreatedAt = r.CreatedAt,
                Username = r.User.Username,
                ProfileImageUrl = r.User.ProfileImageUrl
            });

        // Apply sorting
        query = sort?.ToLower() switch
        {
            "newest" => query.OrderByDescending(r => r.CreatedAt),
            "oldest" => query.OrderBy(r => r.CreatedAt),
            "highest" => query.OrderByDescending(r => r.Rating),
            "lowest" => query.OrderBy(r => r.Rating),
            _ => query.OrderByDescending(r => r.CreatedAt) // Default to newest
        };

        return await query.ToListAsync();
    }
}
