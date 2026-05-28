using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class PageSectionController(ShopMeDbContext context) : Controller
{
    public async Task<IActionResult> Create(int pageId)
    {
        var page = await context.Page.FindAsync(pageId);
        if (page == null)
        {
            return NotFound();
        }

        LoadSectionTypes();
        ViewBag.PageTitle = page.Title;
        return View(new PageSection { PageId = pageId, IsActive = true, SortOrder = await NextSortOrder(pageId) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Type,Title,Content,ImageUrl,ButtonText,ButtonUrl,SortOrder,IsActive,PageId")] PageSection section)
    {
        if (ModelState.IsValid)
        {
            context.Add(section);
            await context.SaveChangesAsync();
            return RedirectToAction("Details", "Page", new { id = section.PageId });
        }

        LoadSectionTypes();
        return View(section);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var section = await context.PageSection.FindAsync(id);
        if (section == null)
        {
            return NotFound();
        }

        LoadSectionTypes();
        return View(section);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Title,Content,ImageUrl,ButtonText,ButtonUrl,SortOrder,IsActive,PageId")] PageSection section)
    {
        if (id != section.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(section);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageSectionExists(section.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction("Details", "Page", new { id = section.PageId });
        }

        LoadSectionTypes();
        return View(section);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var section = await context.PageSection
            .Include(item => item.Page)
            .FirstOrDefaultAsync(item => item.Id == id);

        return section == null ? NotFound() : View(section);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var section = await context.PageSection.FindAsync(id);
        var pageId = section?.PageId;

        if (section != null)
        {
            context.PageSection.Remove(section);
            await context.SaveChangesAsync();
        }

        return pageId == null
            ? RedirectToAction("Index", "Page")
            : RedirectToAction("Details", "Page", new { id = pageId });
    }

    private void LoadSectionTypes()
    {
        ViewBag.SectionTypes = new SelectList(PageSectionTypes.All);
    }

    private async Task<int> NextSortOrder(int pageId)
    {
        var maxSortOrder = await context.PageSection
            .Where(item => item.PageId == pageId)
            .Select(item => (int?)item.SortOrder)
            .MaxAsync();

        return (maxSortOrder ?? 0) + 10;
    }

    private bool PageSectionExists(int id)
    {
        return context.PageSection.Any(item => item.Id == id);
    }
}
