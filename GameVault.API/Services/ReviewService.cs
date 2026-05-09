using AutoMapper;
using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Services;

public class ReviewService
{
    private readonly GameVaultContext _context;
    private readonly IMapper _mapper;

    public ReviewService(GameVaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Get all reviews for a specific game with sorting
    public async Task<List<ReviewDto>> GetReviewsByGameIdAsync(int gameId, string? sort = null)
    {
        var reviews = await _context.Reviews
            .Where(r => r.GameId == gameId)
            .Include(r => r.Game)
            .Include(r => r.User)
            .AsNoTracking()
            .ToListAsync();

        var dtos = _mapper.Map<List<ReviewDto>>(reviews);

        // Apply sorting
        dtos = sort?.ToLower() switch
        {
            "newest" => dtos.OrderByDescending(r => r.CreatedAt).ToList(),
            "oldest" => dtos.OrderBy(r => r.CreatedAt).ToList(),
            "highest" => dtos.OrderByDescending(r => r.Rating).ToList(),
            "lowest" => dtos.OrderBy(r => r.Rating).ToList(),
            _ => dtos.OrderByDescending(r => r.CreatedAt).ToList() // Default to newest
        };

        return dtos;
    }
}
