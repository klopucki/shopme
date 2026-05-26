using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers;

public class RankingController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var rankings = await context.Ranking
            .Where(ranking => ranking.IsPublished && ranking.PublishedAt <= DateTime.Now)
            .OrderByDescending(ranking => ranking.PublishedAt)
            .ToListAsync();

        return View(rankings);
    }

    public async Task<IActionResult> Details(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return NotFound();
        }

        var ranking = await context.Ranking
            .Include(item => item.RankingItems!)
            .FirstOrDefaultAsync(item => item.Slug == slug && item.IsPublished && item.PublishedAt <= DateTime.Now);

        if (ranking == null)
        {
            return NotFound();
        }

        ranking.RankingItems = ranking.RankingItems?
            .OrderBy(item => item.Position)
            .ThenByDescending(item => item.Score)
            .ToList();

        return View(ranking);
    }
}
