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

    public PlatformsController(GameVaultContext context)
    {
        _context = context;
    }

    // GET /api/platforms
    // Returns all platforms
    [HttpGet]
    public async Task<ActionResult<List<PlatformDto>>> GetPlatforms()
    {
        var platforms = await _context.Platforms
            .Select(p => new PlatformDto
            {
                PlatformId = p.PlatformId,
                Name = p.Name
            })
            .ToListAsync();
        return Ok(platforms);
    }
}
