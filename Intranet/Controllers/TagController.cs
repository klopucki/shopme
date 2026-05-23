using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers
{
    public class TagController(ShopMeDbContext context) : Controller
    {
        // GET: Tag
        public async Task<IActionResult> Index()
        {
            return View(await context.Tag.Where(t => t.IsActive).ToListAsync());
        }

        // GET: Tag/Details/5 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await context.Tag
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tag/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                context.Add(tag);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tag/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await context.Tag.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tag/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tagToUpdate = await context.Tag.FirstOrDefaultAsync(t => t.Id == id && t.IsActive);
                    if (tagToUpdate == null)
                    {
                        return NotFound();
                    }
                    tagToUpdate.Name = tag.Name;
                    tagToUpdate.IsActive = tag.IsActive; // Allow editing IsActive if needed, otherwise remove from bind
                    
                    context.Update(tagToUpdate);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await context.Tag
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await context.Tag.FirstOrDefaultAsync(t => t.Id == id);
            if (tag != null)
            {
                tag.IsActive = false;

                var productTags = await context.ProductTag
                    .Where(pt => pt.TagId == id)
                    .ToListAsync();

                foreach (var productTag in productTags)
                {
                    productTag.IsActive = false;
                }
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return context.Tag.Any(e => e.Id == id && e.IsActive);
        }
    }
}
