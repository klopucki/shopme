using Client.Controllers;
using Client.Models;
using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class PageControllerTests
{
    [Fact]
    public async Task Details_returns_not_found_when_slug_is_empty()
    {
        await using var context = CreateContext();
        var controller = new PageController(context);

        var result = await controller.Details(" ");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_returns_published_page_by_slug()
    {
        await using var context = CreateContext();
        context.Page.Add(new Page
        {
            Title = "About us",
            Slug = "about-us",
            IsPublished = true,
            PublishedAt = DateTime.Now.AddDays(-1),
            IsActive = true,
            PageSections =
            [
                new PageSection
                {
                    Type = PageSectionTypes.Text,
                    Title = "Welcome",
                    SortOrder = 1,
                    IsActive = true
                }
            ]
        });
        await context.SaveChangesAsync();

        var controller = new PageController(context);

        var result = await controller.Details("about-us");

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<PageRenderViewModel>(view.Model);
        Assert.Equal("About us", model.Page.Title);
        Assert.Single(model.Page.PageSections!);
    }

    private static ShopMeDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ShopMeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ShopMeDbContext(options);
    }
}
