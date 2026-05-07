using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Services;

public class NewsService
{
    private readonly GameVaultContext _context;

    public NewsService(GameVaultContext context)
    {
        _context = context;
    }

    // Get all published news items sorted by publish date descending
    public async Task<List<NewsSummaryDto>> GetPublishedNewsAsync()
    {
        return await _context.News
            .Where(n => n.IsPublished)
            .OrderByDescending(n => n.PublishDate)
            .Select(n => new NewsSummaryDto
            {
                NewsId = n.NewsId,
                Title = n.Title,
                Content = n.Content,
                PublishDate = n.PublishDate.HasValue ? n.PublishDate.Value : DateTime.MinValue
            })
            .ToListAsync();
    }
}
