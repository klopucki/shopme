using Core.Data;
using Core.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers
{
    public class ProductReviewController(ShopMeDbContext context) : Controller
    {
        // GET: ProductReview
        public async Task<IActionResult> Index()
        {
            var shopMeDbContext = context.ProductReview.Include(p => p.Product);
            return View(await shopMeDbContext.ToListAsync());
        }

        // GET: ProductReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await context.ProductReview
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productReview == null)
            {
                return NotFound();
            }

            return View(productReview);
        }

        // GET: ProductReview/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(context.Product, "Id", "Name");
            return View();
        }

        // POST: ProductReview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,ReviewerName,Rating,Comment,CreatedAt")] ProductReview productReview)
        {
            if (ModelState.IsValid)
            {
                context.Add(productReview);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(context.Product, "Id", "Name", productReview.ProductId);
            return View(productReview);
        }

        // GET: ProductReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await context.ProductReview.FindAsync(id);
            if (productReview == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(context.Product, "Id", "Name", productReview.ProductId);
            return View(productReview);
        }

        // POST: ProductReview/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,ReviewerName,Rating,Comment,CreatedAt")] ProductReview productReview)
        {
            if (id != productReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(productReview);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductReviewExists(productReview.Id))
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
            ViewData["ProductId"] = new SelectList(context.Product, "Id", "Name", productReview.ProductId);
            return View(productReview);
        }

        // GET: ProductReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productReview = await context.ProductReview
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productReview == null)
            {
                return NotFound();
            }

            return View(productReview);
        }

        // POST: ProductReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productReview = await context.ProductReview.FirstOrDefaultAsync(pr => pr.Id == id);
            if (productReview != null)
            {
                context.ProductReview.Remove(productReview);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductReviewExists(int id)
        {
            return context.ProductReview.Any(e => e.Id == id);
        }
    }
}
