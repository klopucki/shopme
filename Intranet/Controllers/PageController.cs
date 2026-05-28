using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class PageController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var pages = await context.Page
            .Include(page => page.PageSections!)
            .OrderBy(page => page.NavigationOrder)
            .ThenBy(page => page.Title)
            .ToListAsync();

        return View(pages);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await context.Page
            .Include(item => item.PageSections!)
            .FirstOrDefaultAsync(item => item.Id == id);

        return page == null ? NotFound() : View(page);
    }

    public IActionResult Create()
    {
        return View(new Page { IsActive = true, IsPublished = true, PublishedAt = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Slug,Summary,IsPublished,PublishedAt,IsActive,ShowInNavigation,NavigationOrder,CreatedAt")] Page page)
    {
        page.Slug = BuildSlug(page.Slug, page.Title);
        page.CreatedAt = page.CreatedAt == default ? DateTime.Now : page.CreatedAt;

        if (ModelState.IsValid)
        {
            context.Add(page);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = page.Id });
        }

        return View(page);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await context.Page.FindAsync(id);
        return page == null ? NotFound() : View(page);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Slug,Summary,IsPublished,PublishedAt,IsActive,ShowInNavigation,NavigationOrder,CreatedAt")] Page page)
    {
        if (id != page.Id)
        {
            return NotFound();
        }

        page.Slug = BuildSlug(page.Slug, page.Title);

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(page);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(page.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Details), new { id = page.Id });
        }

        return View(page);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var page = await context.Page
            .Include(item => item.PageSections!)
            .FirstOrDefaultAsync(item => item.Id == id);

        return page == null ? NotFound() : View(page);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var page = await context.Page
            .Include(item => item.PageSections)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (page != null)
        {
            context.Page.Remove(page);

            if (page.PageSections != null)
            {
                context.PageSection.RemoveRange(page.PageSections);
            }

            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PageExists(int id)
    {
        return context.Page.Any(item => item.Id == id);
    }

    private static string BuildSlug(string? slug, string fallback)
    {
        var source = string.IsNullOrWhiteSpace(slug) ? fallback : slug;
        return string.Join("-", source.Trim().ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}
