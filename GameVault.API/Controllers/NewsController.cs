using GameVault.API.DTOs;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly NewsService _newsService;

    public NewsController(NewsService newsService)
    {
        _newsService = newsService;
    }

    // GET /api/news
    // Returns all published news items sorted by publish date (most recent first)
    [HttpGet]
    public async Task<ActionResult<List<NewsSummaryDto>>> GetNews()
    {
        var news = await _newsService.GetPublishedNewsAsync();
        return Ok(news);
    }
}
