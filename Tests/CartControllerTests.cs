using Client.Controllers;
using Client.Services;
using Core.Data;
using Core.Models.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class CartControllerTests
{
    [Fact]
    public async Task Add_adds_product_to_empty_cart()
    {
        await using var context = CreateContext();
        context.Product.Add(new Product
        {
            Id = 1,
            Name = "Mouse",
            Price = 49.90m,
            ImageUrl = "/mouse.png",
            Quantity = 5,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var session = new FakeSession();
        var controller = CreateController(context, session);

        var result = await controller.Add(1);

        Assert.IsType<RedirectToActionResult>(result);
        var item = Assert.Single(session.GetCart());
        Assert.Equal(1, item.ProductId);
        Assert.Equal("Mouse", item.Name);
        Assert.Equal(49.90m, item.Price);
        Assert.Equal(1, item.Quantity);
    }

    [Fact]
    public async Task Add_increases_quantity_when_product_is_already_in_cart()
    {
        await using var context = CreateContext();
        context.Product.Add(new Product
        {
            Id = 1,
            Name = "Mouse",
            Price = 49.90m,
            Quantity = 5,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var session = new FakeSession();
        var controller = CreateController(context, session);

        await controller.Add(1);
        await controller.Add(1);

        var item = Assert.Single(session.GetCart());
        Assert.Equal(2, item.Quantity);
    }

    [Fact]
    public async Task Add_returns_not_found_when_product_does_not_exist()
    {
        await using var context = CreateContext();
        var controller = CreateController(context, new FakeSession());

        var result = await controller.Add(999);

        Assert.IsType<NotFoundResult>(result);
    }

    private static CartController CreateController(ShopMeDbContext context, ISession session)
    {
        return new CartController(context)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Session = session
                }
            }
        };
    }

    private static ShopMeDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ShopMeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ShopMeDbContext(options);
    }
}
