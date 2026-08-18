using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookMaster.Api.Data;
using BookMaster.Api.Models;
using BookMaster.Api.DTOs;

namespace BookMaster.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _db;
    public BooksController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll([FromQuery] string? search, [FromQuery] long? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(b => b.Title.Contains(search));

        if (categoryId.HasValue)
            query = query.Where(b => b.CategoryId == categoryId.Value);

        var books = await query
            .OrderBy(b => b.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BookDto(b.Id, b.Title, b.OwnerId, b.CategoryId, b.Status))
            .ToListAsync();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(long id)
    {
        var b = await _db.Books.FindAsync(id);
        if (b == null) return NotFound();
        return Ok(new BookDto(b.Id, b.Title, b.OwnerId, b.CategoryId, b.Status));
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookDto dto)
    {
        var ownerExists = await _db.Users.AnyAsync(u => u.Id == dto.OwnerId);
        if (!ownerExists) return BadRequest("Owner does not exist.");

        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists) return BadRequest("Category does not exist.");

        var book = new Book
        {
            Title = dto.Title,
            OwnerId = dto.OwnerId,
            CategoryId = dto.CategoryId,
            Status = BookStatus.Owned
        };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, new BookDto(book.Id, book.Title, book.OwnerId, book.CategoryId, book.Status));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateBookDto dto)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return NotFound();

        book.Title = dto.Title;
        book.CategoryId = dto.CategoryId;
        book.Status = dto.Status;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book == null) return NotFound();

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
