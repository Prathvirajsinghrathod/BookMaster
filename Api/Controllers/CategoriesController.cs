using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookMaster.Api.Data;
using BookMaster.Api.Models;
using BookMaster.Api.DTOs;

namespace BookMaster.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _db.Categories
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(long id)
    {
        var c = await _db.Categories.FindAsync(id);
        if (c == null) return NotFound();
        return Ok(new CategoryDto(c.Id, c.Name));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(CreateCategoryDto dto)
    {
        var category = new Category { Name = dto.Name };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, new CategoryDto(category.Id, category.Name));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, CreateCategoryDto dto)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();

        category.Name = dto.Name;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
