using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Services;

public class WebResourceService
{
    private readonly GameVaultContext _context;

    public WebResourceService(GameVaultContext context)
    {
        _context = context;
    }

    // Get all web resources
    public async Task<List<WebResourceDto>> GetAllWebResourcesAsync()
    {
        return await _context.WebResources
            .Select(w => new WebResourceDto
            {
                ResourceId = w.ResourceId,
                Title = w.Title,
                HtmlContent = w.HtmlContent
            })
            .ToListAsync();
    }

    // Get a single web resource by ID
    public async Task<WebResourceDto?> GetWebResourceByIdAsync(int id)
    {
        return await _context.WebResources
            .Where(w => w.ResourceId == id)
            .Select(w => new WebResourceDto
            {
                ResourceId = w.ResourceId,
                Title = w.Title,
                HtmlContent = w.HtmlContent
            })
            .FirstOrDefaultAsync();
    }
}
