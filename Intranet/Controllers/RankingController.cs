using Core.Data;
using Core.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class RankingController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.Ranking.OrderByDescending(item => item.PublishedAt).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ranking = await context.Ranking
            .Include(item => item.RankingItems!)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (ranking != null)
        {
            ranking.RankingItems = ranking.RankingItems?.OrderBy(item => item.Position).ToList();
        }

        return ranking == null ? NotFound() : View(ranking);
    }

    public IActionResult Create()
    {
        return View(new Ranking { IsActive = true, IsPublished = true, PublishedAt = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Slug,Summary,IsFeatured,IsPublished,PublishedAt,IsActive")] Ranking ranking)
    {
        ranking.Slug = BuildSlug(ranking.Slug, ranking.Title);

        if (ModelState.IsValid)
        {
            context.Add(ranking);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(ranking);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ranking = await context.Ranking.FindAsync(id);
        return ranking == null ? NotFound() : View(ranking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Slug,Summary,IsFeatured,IsPublished,PublishedAt,IsActive")] Ranking ranking)
    {
        if (id != ranking.Id)
        {
            return NotFound();
        }

        ranking.Slug = BuildSlug(ranking.Slug, ranking.Title);

        if (ModelState.IsValid)
        {
            context.Update(ranking);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(ranking);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ranking = await context.Ranking.FirstOrDefaultAsync(item => item.Id == id);
        return ranking == null ? NotFound() : View(ranking);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ranking = await context.Ranking
            .Include(item => item.RankingItems)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (ranking != null)
        {
            context.Ranking.Remove(ranking);

            if (ranking.RankingItems != null)
            {
                context.RankingItem.RemoveRange(ranking.RankingItems);
            }

            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private static string BuildSlug(string? slug, string fallback)
    {
        var source = string.IsNullOrWhiteSpace(slug) ? fallback : slug;
        return string.Join("-", source.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
