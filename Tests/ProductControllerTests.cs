using Client.Controllers;
using Client.Models;
using Core.Data;
using Core.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class ProductControllerTests
{
    [Fact]
    public async Task Index_filters_products_by_search_text()
    {
        await using var context = CreateContext();
        context.Category.Add(new Category { Id = 1, Name = "Accessories" });
        context.Product.AddRange(
            new Product
            {
                Name = "Gaming Mouse",
                Price = 99m,
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 1, 2),
                IsActive = true
            },
            new Product
            {
                Name = "Office Keyboard",
                Price = 149m,
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 1, 1),
                IsActive = true
            });
        await context.SaveChangesAsync();

        var controller = new ProductController(context);

        var result = await controller.Index(categoryId: null, tagId: null, search: "Mouse");

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ProductListViewModel>(view.Model);
        var product = Assert.Single(model.Products);
        Assert.Equal("Gaming Mouse", product.Name);
        Assert.Equal("Mouse", model.Search);
    }

    [Fact]
    public async Task Details_returns_not_found_when_id_is_missing()
    {
        await using var context = CreateContext();
        var controller = new ProductController(context);

        var result = await controller.Details(null);

        Assert.IsType<NotFoundResult>(result);
    }

    private static ShopMeDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ShopMeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ShopMeDbContext(options);
    }
}
