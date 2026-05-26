using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers;

public class ArticleController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var articles = await context.Article
            .Include(article => article.ArticleCategory)
            .Where(article => article.IsPublished && article.PublishedAt <= DateTime.Now)
            .OrderByDescending(article => article.PublishedAt)
            .ToListAsync();

        return View(articles);
    }

    public async Task<IActionResult> Details(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var article = await context.Article
            .Include(item => item.ArticleCategory)
            .Include(item => item.ArticleTagAssignments!)
                .ThenInclude(item => item.ArticleTag)
            .FirstOrDefaultAsync(item => item.Slug == slug && item.IsPublished && item.PublishedAt <= DateTime.Now);

        if (article == null)
        {
            return NotFound();
        }

        return View(article);
    }
}
