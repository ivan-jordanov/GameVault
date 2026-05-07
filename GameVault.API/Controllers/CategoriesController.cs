using GameVault.API.Data;
using GameVault.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly GameVaultContext _context;

    public CategoriesController(GameVaultContext context)
    {
        _context = context;
    }

    // GET /api/categories
    // Returns all categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            })
            .ToListAsync();
        return Ok(categories);
    }
}
