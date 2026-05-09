using AutoMapper;
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
    private readonly IMapper _mapper;

    public CategoriesController(GameVaultContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET /api/categories
    // Returns all categories
    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .ToListAsync();
        
        var dtos = _mapper.Map<List<CategoryDto>>(categories);
        return Ok(dtos);
    }
}
