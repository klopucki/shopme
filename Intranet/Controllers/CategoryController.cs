using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers
{
    public class CategoryController(ShopMeDbContext context) : Controller
    {
        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await context.Category.Where(c => c.IsActive).ToListAsync());
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                context.Add(category);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await context.Category.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsActive")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoryToUpdate = await context.Category.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
                    if (categoryToUpdate == null)
                    {
                        return NotFound();
                    }
                    categoryToUpdate.Name = category.Name;
                    categoryToUpdate.IsActive = category.IsActive; // Allow editing IsActive if needed
                    
                    context.Update(categoryToUpdate);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await context.Category
                .FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await context.Category.FirstOrDefaultAsync(c => c.Id == id);
            if (category != null)
            {
                category.IsActive = false;

                var products = await context.Product
                    .Include(p => p.ProductTags)
                    .Include(p => p.ProductDetails)
                    .Include(p => p.ProductReviews)
                    .Where(p => p.CategoryId == id)
                    .ToListAsync();

                foreach (var product in products)
                {
                    product.IsActive = false;

                    if (product.ProductTags != null)
                    {
                        foreach (var productTag in product.ProductTags)
                        {
                            productTag.IsActive = false;
                        }
                    }

                    if (product.ProductDetails != null)
                    {
                        product.ProductDetails.IsActive = false;
                    }

                    if (product.ProductReviews != null)
                    {
                        foreach (var productReview in product.ProductReviews)
                        {
                            productReview.IsActive = false;
                        }
                    }
                }
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return context.Category.Any(e => e.Id == id && e.IsActive);
        }
    }
}
