using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookMaster.Api.Data;
using BookMaster.Api.DTOs;

namespace BookMaster.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly AppDbContext _db;
    public HistoryController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HistoryDto>>> GetAll()
    {
        var history = await _db.History
            .Select(h => new HistoryDto(h.Id, h.RequestId, h.CompletedAt))
            .ToListAsync();
        return Ok(history);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HistoryDto>> GetById(long id)
    {
        var h = await _db.History.FindAsync(id);
        if (h == null) return NotFound();
        return Ok(new HistoryDto(h.Id, h.RequestId, h.CompletedAt));
    }
}
