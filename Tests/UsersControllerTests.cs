using Microsoft.AspNetCore.Mvc;
using BookMaster.Api.Controllers;
using BookMaster.Api.DTOs;
using Xunit;

namespace BookMaster.Tests;

public class UsersControllerTests
{
    [Fact]
    public async Task Create_ReturnsCreated_WhenEmailIsUnique()
    {
        var db = TestDbFactory.Create();
        var controller = new UsersController(db);

        var result = await controller.Create(new CreateUserDto("Alice", "alice@example.com"));

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var dto = Assert.IsType<UserDto>(created.Value);
        Assert.Equal("Alice", dto.Name);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var db = TestDbFactory.Create();
        var controller = new UsersController(db);

        await controller.Create(new CreateUserDto("Alice", "alice@example.com"));
        var result = await controller.Create(new CreateUserDto("Bob", "alice@example.com"));

        Assert.IsType<ConflictObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenUserMissing()
    {
        var db = TestDbFactory.Create();
        var controller = new UsersController(db);

        var result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
