using Client.Models;
using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers;

public class PageController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Details(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var page = await context.Page
            .Include(item => item.PageSections!)
            .FirstOrDefaultAsync(item => item.Slug == slug && item.IsPublished && item.PublishedAt <= DateTime.Now);

        if (page == null)
        {
            return NotFound();
        }

        var model = new PageRenderViewModel
        {
            Page = page,
            FeaturedProducts = await context.Product
                .Include(product => product.Category)
                .Where(product => product.IsFeatured)
                .OrderByDescending(product => product.CreatedAt)
                .Take(4)
                .ToListAsync(),
            LatestArticles = await context.Article
                .Include(article => article.ArticleCategory)
                .Where(article => article.IsPublished && article.PublishedAt <= DateTime.Now)
                .OrderByDescending(article => article.PublishedAt)
                .Take(3)
                .ToListAsync(),
            FeaturedRankings = await context.Ranking
                .Include(ranking => ranking.RankingItems!)
                .Where(ranking => ranking.IsPublished && ranking.PublishedAt <= DateTime.Now)
                .OrderByDescending(ranking => ranking.IsFeatured)
                .ThenByDescending(ranking => ranking.PublishedAt)
                .Take(2)
                .ToListAsync()
        };

        return View(model);
    }
}
