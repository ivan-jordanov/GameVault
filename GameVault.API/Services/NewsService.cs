using AutoMapper;
using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Services;

public class NewsService
{
    private readonly GameVaultContext _context;
    private readonly IMapper _mapper;

    public NewsService(GameVaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Get all published news items sorted by publish date descending
    public async Task<List<NewsSummaryDto>> GetPublishedNewsAsync()
    {
        var news = await _context.News
            .Where(n => n.IsPublished)
            .OrderByDescending(n => n.PublishDate)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<NewsSummaryDto>>(news);
    }
}
