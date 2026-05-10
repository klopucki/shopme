// using Core.Data;
// using Core.Models.CMS;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace Intranet.Controllers;
//
// public class ProductTagController(ShopMeDbContext context) : Controller
// {
//     // GET: ProductTag
//     public async Task<IActionResult> Index()
//     {
//         return View(await context.Category.ToListAsync());
//     }
//
//     // GET: ProductTag/Details/5
//     public async Task<IActionResult> Details(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }
//
//         var category = await context.Category
//             .FirstOrDefaultAsync(m => m.Id == id);
//         if (category == null)
//         {
//             return NotFound();
//         }
//
//         return View(category);
//     }
//
//     // GET: ProductTag/Create
//     public IActionResult Create()
//     {
//         return View();
//     }
//
//     // POST: ProductTag/Create
//     // To protect from overposting attacks, enable the specific properties you want to bind to.
//     // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
//     {
//         if (ModelState.IsValid)
//         {
//             context.Add(category);
//             await context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//         return View(category);
//     }
//
//     // GET: ProductTag/Edit/5
//     public async Task<IActionResult> Edit(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }
//
//         var category = await context.Category.FindAsync(id);
//         if (category == null)
//         {
//             return NotFound();
//         }
//         return View(category);
//     }
//
//     // POST: ProductTag/Edit/5
//     // To protect from overposting attacks, enable the specific properties you want to bind to.
//     // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
//     {
//         if (id != category.Id)
//         {
//             return NotFound();
//         }
//
//         if (ModelState.IsValid)
//         {
//             try
//             {
//                 context.Update(category);
//                 await context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!CategoryExists(category.Id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }
//             return RedirectToAction(nameof(Index));
//         }
//         return View(category);
//     }
//
//     // GET: ProductTag/Delete/5
//     public async Task<IActionResult> Delete(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }
//
//         var category = await context.Category
//             .FirstOrDefaultAsync(m => m.Id == id);
//         if (category == null)
//         {
//             return NotFound();
//         }
//
//         return View(category);
//     }
//
//     // POST: ProductTag/Delete/5
//     [HttpPost, ActionName("Delete")]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> DeleteConfirmed(int id)
//     {
//         var category = await context.Category.FindAsync(id);
//         if (category != null)
//         {
//             context.Category.Remove(category);
//         }
//
//         await context.SaveChangesAsync();
//         return RedirectToAction(nameof(Index));
//     }
//
//     private bool CategoryExists(int id)
//     {
//         return context.Category.Any(e => e.Id == id);
//     }
// }