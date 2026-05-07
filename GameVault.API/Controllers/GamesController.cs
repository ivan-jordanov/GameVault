using GameVault.API.DTOs;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameService _gameService;

    public GamesController(GameService gameService)
    {
        _gameService = gameService;
    }

    // GET /api/games/search
    // Searches games by title and optionally filters by category and platform
    [HttpGet("search")]
    public async Task<ActionResult<List<GameSummaryDto>>> SearchGames(
        [FromQuery] string? q,
        [FromQuery] int? categoryId = null,
        [FromQuery] int? platformId = null)
    {
        var results = await _gameService.SearchGamesAsync(q, categoryId, platformId);
        return Ok(results);
    }

    // GET /api/games/{id}
    // Returns detailed information about a single game
    [HttpGet("{id}")]
    public async Task<ActionResult<GameDetailDto>> GetGameById(int id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }

    // GET /api/games
    // Returns all games, optionally sorted by rating, releasedate, or alphabetical
    [HttpGet]
    public async Task<ActionResult<List<GameSummaryDto>>> GetGames([FromQuery] string? sort)
    {
        var games = await _gameService.GetAllGamesAsync(sort);
        return Ok(games);
    }
}
