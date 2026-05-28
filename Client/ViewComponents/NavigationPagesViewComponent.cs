using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.ViewComponents;

public class NavigationPagesViewComponent(ShopMeDbContext context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var pages = await context.Page
            .Where(page => page.IsPublished && page.PublishedAt <= DateTime.Now && page.ShowInNavigation)
            .OrderBy(page => page.NavigationOrder)
            .ThenBy(page => page.Title)
            .ToListAsync();

        return View(pages);
    }
}
