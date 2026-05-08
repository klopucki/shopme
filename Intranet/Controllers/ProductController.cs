using Intranet.Data;
using Intranet.Models.CMS;
using Intranet.Models.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers;

public class ProductController(IntranetContext context) : Controller
{
    // GET: asda
    public async Task<IActionResult> Index()
    {
        var intranetContext = context.Product.Include(p => p.Category);
        return View(await intranetContext.ToListAsync());
    }

    // GET: asda/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Product
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: asda/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(context.Set<Category>(), "Id", "Name");
        return View();
    }

    // POST: asda/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,IsActive,CreatedAt,CategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            context.Add(product);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(context.Set<Category>(), "Id", "Name", product.CategoryId);
        return View(product);
    }

    // GET: asda/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Product.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(context.Set<Category>(), "Id", "Name", product.CategoryId);
        return View(product);
    }

    // POST: asda/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,IsActive,CreatedAt,CategoryId")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(product);
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
        ViewData["CategoryId"] = new SelectList(context.Set<Category>(), "Id", "Name", product.CategoryId);
        return View(product);
    }

    // GET: asda/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Product
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: asda/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await context.Product.FindAsync(id);
        if (product != null)
        {
            context.Product.Remove(product);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return context.Product.Any(e => e.Id == id);
    }
}