using GameVault.API.DTOs;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ReviewService _reviewService;

    public ReviewsController(ReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // GET /api/reviews/game/{gameId}
    // Returns all reviews for a specific game, optionally sorted by newest, oldest, highest, or lowest
    [HttpGet("game/{gameId}")]
    public async Task<ActionResult<List<ReviewDto>>> GetReviewsByGameId(int gameId, [FromQuery] string? sort)
    {
        var reviews = await _reviewService.GetReviewsByGameIdAsync(gameId, sort);
        return Ok(reviews);
    }
}
