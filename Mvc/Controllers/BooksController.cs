using Microsoft.AspNetCore.Mvc;
using BookMaster.Mvc.Models;
using BookMaster.Mvc.Services;

namespace BookMaster.Mvc.Controllers;

public class BooksController : Controller
{
    private readonly BookApiClient _api;
    public BooksController(BookApiClient api) => _api = api;

    public async Task<IActionResult> Index(string? search, long? categoryId)
    {
        ViewBag.Categories = await _api.GetCategoriesAsync();
        var books = await _api.GetBooksAsync(search, categoryId);
        return View(books);
    }

    public async Task<IActionResult> Details(long id)
    {
        var book = await _api.GetBookAsync(id);
        if (book == null) return NotFound();
        return View(book);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Users = await _api.GetUsersAsync();
        ViewBag.Categories = await _api.GetCategoriesAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookVm book)
    {
        var response = await _api.CreateBookAsync(book);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Could not create book.");
            ViewBag.Users = await _api.GetUsersAsync();
            ViewBag.Categories = await _api.GetCategoriesAsync();
            return View(book);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long id)
    {
        await _api.DeleteBookAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
