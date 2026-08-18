using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookMaster.Api.Data;
using BookMaster.Api.Models;
using BookMaster.Api.DTOs;

namespace BookMaster.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    public UsersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _db.Users
            .Select(u => new UserDto(u.Id, u.Name, u.Email))
            .ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(long id)
    {
        var u = await _db.Users.FindAsync(id);
        if (u == null) return NotFound();
        return Ok(new UserDto(u.Id, u.Name, u.Email));
    }

    [HttpGet("{id}/library")]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetLibrary(long id)
    {
        var exists = await _db.Users.AnyAsync(u => u.Id == id);
        if (!exists) return NotFound();

        var books = await _db.Books
            .Where(b => b.OwnerId == id)
            .Select(b => new BookDto(b.Id, b.Title, b.OwnerId, b.CategoryId, b.Status))
            .ToListAsync();
        return Ok(books);
    }

    [HttpGet("{id}/notifications")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications(long id)
    {
        var notes = await _db.Notifications
            .Where(n => n.UserId == id)
            .Select(n => new NotificationDto(n.Id, n.UserId, n.IsRead))
            .ToListAsync();
        return Ok(notes);
    }

    [HttpGet("{id}/exchange-history")]
    public async Task<ActionResult<IEnumerable<HistoryDto>>> GetExchangeHistory(long id)
    {
        var history = await _db.History
            .Include(h => h.Request!).ThenInclude(r => r.Listing!).ThenInclude(l => l.Book)
            .Where(h => h.Request!.RequesterId == id
                || h.Request!.Listing!.Book!.OwnerId == id)
            .Select(h => new HistoryDto(h.Id, h.RequestId, h.CompletedAt))
            .ToListAsync();
        return Ok(history);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            return Conflict("Email already registered.");

        var user = new User { Name = dto.Name, Email = dto.Email };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, new UserDto(user.Id, user.Name, user.Email));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, CreateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Name = dto.Name;
        user.Email = dto.Email;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
