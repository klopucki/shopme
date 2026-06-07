using Core.Data;
using Core.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers
{
    public class CategoryController(ShopMeDbContext context) : Controller
    {
        // GET: Category
        public async Task<IActionResult> Index()
        {
            return View(await context.Category.ToListAsync());
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

            var category = await context.Category.FindAsync(id);
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
                    var categoryToUpdate = await context.Category.FindAsync(id);
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

            var category = await context.Category.FirstOrDefaultAsync(m => m.Id == id);
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
                var products = await context.Product
                    .Include(p => p.ProductTags)
                    .Include(p => p.ProductDetails)
                    .Include(p => p.ProductReviews)
                    .Where(p => p.CategoryId == id)
                    .ToListAsync();

                context.Category.Remove(category);
                context.Product.RemoveRange(products);

                foreach (var product in products)
                {
                    if (product.ProductTags != null)
                    {
                        context.ProductTag.RemoveRange(product.ProductTags);
                    }

                    if (product.ProductDetails != null)
                    {
                        context.ProductDetails.Remove(product.ProductDetails);
                    }

                    if (product.ProductReviews != null)
                    {
                        context.ProductReview.RemoveRange(product.ProductReviews);
                    }
                }
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return context.Category.Any(e => e.Id == id);
        }
    }
}
