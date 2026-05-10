// using Core.Data;
// using Core.Models.CMS;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.EntityFrameworkCore;
//
// namespace Intranet.Controllers;
//
// public class ProductDetailsController : Controller
// {
//     private readonly ShopMeDbContext _context;
//
//     public ProductDetailsController(ShopMeDbContext context)
//     {
//         _context = context;
//     }
//
//     // GET: ProductDetails
//     public async Task<IActionResult> Index()
//     {
//         var intranetContext = _context.ProductDetails.Include(p => p.Product);
//         return View(await intranetContext.ToListAsync());
//     }
//
//     // GET: ProductDetails/Details/5
//     public async Task<IActionResult> Details(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }
//
//         var productDetails = await _context.ProductDetails
//             .Include(p => p.Product)
//             .FirstOrDefaultAsync(m => m.Id == id);
//         if (productDetails == null)
//         {
//             return NotFound();
//         }
//
//         return View(productDetails);
//     }
//
//     // GET: ProductDetails/Create
//     public IActionResult Create()
//     {
//         ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
//         return View();
//     }
//
//     // POST: ProductDetails/Create
//     // To protect from overposting attacks, enable the specific properties you want to bind to.
//     // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create([Bind("Id,Description,Manufacturer,Weight,ProductId")] ProductDetails productDetails)
//     {
//         if (ModelState.IsValid)
//         {
//             _context.Add(productDetails);
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//         ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productDetails.ProductId);
//         return View(productDetails);
//     }
//
//     // GET: ProductDetails/Edit/5
//     public async Task<IActionResult> Edit(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }
//
//         var productDetails = await _context.ProductDetails.FindAsync(id);
//         if (productDetails == null)
//         {
//             return NotFound();
//         }
//         ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productDetails.ProductId);
//         return View(productDetails);
//     }
//
//     // POST: ProductDetails/Edit/5
//     // To protect from overposting attacks, enable the specific properties you want to bind to.
//     // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Manufacturer,Weight,ProductId")] ProductDetails productDetails)
//     {
//         if (id != productDetails.Id)
//         {
//             return NotFound();
//         }
//
//         if (ModelState.IsValid)
//         {
//             try
//             {
//                 _context.Update(productDetails);
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!ProductDetailsExists(productDetails.Id))
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
//         ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", productDetails.ProductId);
//         return View(productDetails);
//     }
//
//     // GET: ProductDetails/Delete/5
//     public async Task<IActionResult> Delete(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }
//
//         var productDetails = await _context.ProductDetails
//             .Include(p => p.Product)
//             .FirstOrDefaultAsync(m => m.Id == id);
//         if (productDetails == null)
//         {
//             return NotFound();
//         }
//
//         return View(productDetails);
//     }
//
//     // POST: ProductDetails/Delete/5
//     [HttpPost, ActionName("Delete")]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> DeleteConfirmed(int id)
//     {
//         var productDetails = await _context.ProductDetails.FindAsync(id);
//         if (productDetails != null)
//         {
//             _context.ProductDetails.Remove(productDetails);
//         }
//
//         await _context.SaveChangesAsync();
//         return RedirectToAction(nameof(Index));
//     }
//
//     private bool ProductDetailsExists(int id)
//     {
//         return _context.ProductDetails.Any(e => e.Id == id);
//     }
// }