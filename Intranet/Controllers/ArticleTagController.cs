using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class ArticleTagController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.ArticleTag.OrderBy(item => item.Name).ToListAsync());
    }

    public IActionResult Create()
    {
        return View(new ArticleTag { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,IsActive")] ArticleTag articleTag)
    {
        if (ModelState.IsValid)
        {
            context.Add(articleTag);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(articleTag);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var articleTag = await context.ArticleTag.FindAsync(id);
        return articleTag == null ? NotFound() : View(articleTag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive")] ArticleTag articleTag)
    {
        if (id != articleTag.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            context.Update(articleTag);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(articleTag);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var articleTag = await context.ArticleTag.FirstOrDefaultAsync(item => item.Id == id);
        return articleTag == null ? NotFound() : View(articleTag);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var articleTag = await context.ArticleTag.FindAsync(id);
        if (articleTag != null)
        {
            context.ArticleTag.Remove(articleTag);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
