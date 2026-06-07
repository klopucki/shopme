using Core.Data;
using Core.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intranet.Controllers
{
    public class ProductController(ShopMeDbContext context) : Controller
    {
        // GET: Product
        public async Task<IActionResult> Index()
        {
            var shopMeDbContext = context.Product.Include(p => p.Category);
            return View(await shopMeDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductTags!)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(context.Category, "Id", "Name");
            ViewBag.Tags = await context.Tag.ToListAsync(); 
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,IsActive,CreatedAt,ActiveUntil,Quantity,IsFeatured,ImageUrl,CategoryId")] Product product, List<int>? selectedTagIds)
        {
            if (ModelState.IsValid)
            {
                context.Add(product);
                await context.SaveChangesAsync(); 

                if (selectedTagIds != null)
                {
                    foreach (var tagId in selectedTagIds)
                    {
                        product.ProductTags ??= new List<ProductTag>();
                        product.ProductTags.Add(new ProductTag { ProductId = product.Id, TagId = tagId });
                    }
                    await context.SaveChangesAsync(); // Save changes to ProductTags
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(context.Category, "Id", "Name", product.CategoryId);
            ViewBag.Tags = await context.Tag.ToListAsync();
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Product
                .Include(p => p.ProductTags!)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(context.Category, "Id", "Name", product.CategoryId);
            
            ViewBag.Tags = await context.Tag.ToListAsync(); // Pass all tags
            ViewBag.SelectedTagIds = product.ProductTags?.Select(pt => pt.TagId).ToList() ?? new List<int>(); 

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,IsActive,CreatedAt,ActiveUntil,Quantity,IsFeatured,ImageUrl,CategoryId")] Product product, List<int>? selectedTagIds)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productToUpdate = await context.Product
                        .Include(p => p.ProductTags)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (productToUpdate == null)
                    {
                        return NotFound();
                    }

                    context.Entry(productToUpdate).CurrentValues.SetValues(product);

                    selectedTagIds ??= new List<int>();
                    var allProductTags = await context.ProductTag
                        .IgnoreQueryFilters()
                        .Where(pt => pt.ProductId == productToUpdate.Id)
                        .ToListAsync();
                    var existingTagIds = allProductTags.Select(pt => pt.TagId).ToList();

                    foreach (var productTag in allProductTags)
                    {
                        productTag.IsActive = selectedTagIds.Contains(productTag.TagId);
                    }

                    foreach (var tagId in selectedTagIds)
                    {
                        if (!existingTagIds.Contains(tagId))
                        {
                            context.ProductTag.Add(new ProductTag { ProductId = productToUpdate.Id, TagId = tagId });
                        }
                    }

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(context.Category, "Id", "Name", product.CategoryId);
            
            ViewBag.Tags = await context.Tag.ToListAsync(); 
            ViewBag.SelectedTagIds = selectedTagIds ?? new List<int>();

            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductTags!)
                    .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await context.Product
                .Include(p => p.ProductTags)
                .Include(p => p.ProductDetails)
                .Include(p => p.ProductReviews)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                context.Product.Remove(product);

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

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return context.Product.Any(e => e.Id == id);
        }
    }
}
