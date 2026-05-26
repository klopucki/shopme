using System.Diagnostics;
using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers;

public class HomeController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new HomePageViewModel
        {
            Categories = await context.Category
                .OrderBy(category => category.Name)
                .Take(4)
                .ToListAsync(),
            FeaturedProducts = await context.Product
                .Include(product => product.Category)
                .Where(product => product.IsFeatured)
                .OrderByDescending(product => product.CreatedAt)
                .Take(4)
                .ToListAsync(),
            LatestArticles = await context.Article
                .Include(article => article.ArticleCategory)
                .Where(article => article.IsPublished && article.PublishedAt <= DateTime.Now)
                .OrderByDescending(article => article.IsFeatured)
                .ThenByDescending(article => article.PublishedAt)
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

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
