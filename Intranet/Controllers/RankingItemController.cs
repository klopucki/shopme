using Core.Data;
using Core.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class RankingItemController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var rankingItems = await context.RankingItem
            .Include(item => item.Ranking)
            .OrderBy(item => item.Ranking!.Title)
            .ThenBy(item => item.Position)
            .ToListAsync();

        return View(rankingItems);
    }

    public async Task<IActionResult> Create(int? rankingId)
    {
        await LoadRankings(rankingId);
        return View(new RankingItem { IsActive = true, RankingId = rankingId ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Summary,Url,Score,Position,IsActive,RankingId")] RankingItem rankingItem)
    {
        if (ModelState.IsValid)
        {
            context.Add(rankingItem);
            await context.SaveChangesAsync();
            return RedirectToAction("Details", "Ranking", new { id = rankingItem.RankingId });
        }

        await LoadRankings(rankingItem.RankingId);
        return View(rankingItem);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var rankingItem = await context.RankingItem.FindAsync(id);
        if (rankingItem == null)
        {
            return NotFound();
        }

        await LoadRankings(rankingItem.RankingId);
        return View(rankingItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Summary,Url,Score,Position,IsActive,RankingId")] RankingItem rankingItem)
    {
        if (id != rankingItem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            context.Update(rankingItem);
            await context.SaveChangesAsync();
            return RedirectToAction("Details", "Ranking", new { id = rankingItem.RankingId });
        }

        await LoadRankings(rankingItem.RankingId);
        return View(rankingItem);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var rankingItem = await context.RankingItem
            .Include(item => item.Ranking)
            .FirstOrDefaultAsync(item => item.Id == id);

        return rankingItem == null ? NotFound() : View(rankingItem);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var rankingItem = await context.RankingItem.FindAsync(id);
        var rankingId = rankingItem?.RankingId;

        if (rankingItem != null)
        {
            context.RankingItem.Remove(rankingItem);
            await context.SaveChangesAsync();
        }

        return rankingId.HasValue
            ? RedirectToAction("Details", "Ranking", new { id = rankingId.Value })
            : RedirectToAction(nameof(Index));
    }

    private async Task LoadRankings(int? selectedId = null)
    {
        ViewData["RankingId"] = new SelectList(await context.Ranking.OrderBy(item => item.Title).ToListAsync(), "Id", "Title", selectedId);
    }
}
