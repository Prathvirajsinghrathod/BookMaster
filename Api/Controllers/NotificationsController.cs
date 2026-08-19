using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookMaster.Api.Data;
using BookMaster.Api.DTOs;

namespace BookMaster.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _db;
    public NotificationsController(AppDbContext db) => _db = db;

    [HttpGet("{id}")]
    public async Task<ActionResult<NotificationDto>> GetById(long id)
    {
        var n = await _db.Notifications.FindAsync(id);
        if (n == null) return NotFound();
        return Ok(new NotificationDto(n.Id, n.UserId, n.IsRead));
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkRead(long id)
    {
        var n = await _db.Notifications.FindAsync(id);
        if (n == null) return NotFound();

        n.IsRead = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
