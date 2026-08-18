using Microsoft.AspNetCore.Mvc;
using BookMaster.Api.Controllers;
using BookMaster.Api.Data;
using BookMaster.Api.DTOs;
using BookMaster.Api.Models;
using Xunit;

namespace BookMaster.Tests;

public class BooksControllerTests
{
    private static async Task<(AppDbContext db, User user, Category category)> Seed()
    {
        var db = TestDbFactory.Create();
        var user = new User { Name = "Alice", Email = "alice@example.com" };
        var category = new Category { Name = "Programming" };
        db.Users.Add(user);
        db.Categories.Add(category);
        await db.SaveChangesAsync();
        return (db, user, category);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenOwnerMissing()
    {
        var (db, _, category) = await Seed();
        var controller = new BooksController(db);

        var result = await controller.Create(new CreateBookDto("Clean Code", 999, category.Id));

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Create_SetsStatusOwned_WhenValid()
    {
        var (db, user, category) = await Seed();
        var controller = new BooksController(db);

        var result = await controller.Create(new CreateBookDto("Clean Code", user.Id, category.Id));

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var dto = Assert.IsType<BookDto>(created.Value);
        Assert.Equal(BookStatus.Owned, dto.Status);
    }

    [Fact]
    public async Task GetAll_FiltersBySearchTerm()
    {
        var (db, user, category) = await Seed();
        db.Books.Add(new Book { Title = "Clean Code", OwnerId = user.Id, CategoryId = category.Id, Status = BookStatus.Owned });
        db.Books.Add(new Book { Title = "Effective Java", OwnerId = user.Id, CategoryId = category.Id, Status = BookStatus.Owned });
        await db.SaveChangesAsync();

        var controller = new BooksController(db);
        var result = await controller.GetAll(search: "Clean", categoryId: null);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var books = Assert.IsAssignableFrom<IEnumerable<BookDto>>(ok.Value);
        Assert.Single(books);
    }
}
