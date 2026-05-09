using AutoMapper;
using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly GameVaultContext _context;
    private readonly IMapper _mapper;

    public PlatformsController(GameVaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET /api/platforms
    // Returns all platforms
    [HttpGet]
    public async Task<ActionResult<List<PlatformDto>>> GetPlatforms()
    {
        var platforms = await _context.Platforms
            .AsNoTracking()
            .ToListAsync();
        
        var dtos = _mapper.Map<List<PlatformDto>>(platforms);
        return Ok(dtos);
    }
}
