using GameVault.API.DTOs;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WebResourcesController : ControllerBase
{
    private readonly WebResourceService _webResourceService;

    public WebResourcesController(WebResourceService webResourceService)
    {
        _webResourceService = webResourceService;
    }

    // GET /api/webresources
    // Returns all web resources
    [HttpGet]
    public async Task<ActionResult<List<WebResourceDto>>> GetWebResources()
    {
        var resources = await _webResourceService.GetAllWebResourcesAsync();
        return Ok(resources);
    }

    // GET /api/webresources/{id}
    // Returns a single web resource by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<WebResourceDto>> GetWebResourceById(int id)
    {
        var resource = await _webResourceService.GetWebResourceByIdAsync(id);
        if (resource == null)
        {
            return NotFound();
        }
        return Ok(resource);
    }
}
