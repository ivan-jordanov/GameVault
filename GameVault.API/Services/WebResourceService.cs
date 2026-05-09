using AutoMapper;
using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Services;

public class WebResourceService
{
    private readonly GameVaultContext _context;
    private readonly IMapper _mapper;

    public WebResourceService(GameVaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Get all web resources
    public async Task<List<WebResourceDto>> GetAllWebResourcesAsync()
    {
        var resources = await _context.WebResources
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<WebResourceDto>>(resources);
    }

    // Get a single web resource by ID
    public async Task<WebResourceDto?> GetWebResourceByIdAsync(int id)
    {
        var resource = await _context.WebResources
            .Where(w => w.ResourceId == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (resource == null)
            return null;

        return _mapper.Map<WebResourceDto>(resource);
    }
}
