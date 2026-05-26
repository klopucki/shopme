using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class ArticleCategoryController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.ArticleCategory.OrderBy(item => item.Name).ToListAsync());
    }

    public IActionResult Create()
    {
        return View(new ArticleCategory { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Slug,IsActive")] ArticleCategory articleCategory)
    {
        articleCategory.Slug = BuildSlug(articleCategory.Slug, articleCategory.Name);

        if (ModelState.IsValid)
        {
            context.Add(articleCategory);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(articleCategory);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var articleCategory = await context.ArticleCategory.FindAsync(id);
        return articleCategory == null ? NotFound() : View(articleCategory);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Slug,IsActive")] ArticleCategory articleCategory)
    {
        if (id != articleCategory.Id)
        {
            return NotFound();
        }

        articleCategory.Slug = BuildSlug(articleCategory.Slug, articleCategory.Name);

        if (ModelState.IsValid)
        {
            context.Update(articleCategory);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(articleCategory);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var articleCategory = await context.ArticleCategory.FirstOrDefaultAsync(item => item.Id == id);
        return articleCategory == null ? NotFound() : View(articleCategory);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var articleCategory = await context.ArticleCategory.FindAsync(id);
        if (articleCategory != null)
        {
            context.ArticleCategory.Remove(articleCategory);
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
