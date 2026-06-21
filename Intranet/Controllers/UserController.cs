using Core.Data;
using Core.Models.CMS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// Removed using System.Security.Cryptography;
// Removed using System.Text;

namespace Intranet.Controllers
{
    public class UserController(ShopMeDbContext context) : Controller
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await context.User.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Email,IsActive")] User user, string password) // Removed PasswordHash from Bind
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(nameof(password), "The Password field is required.");
            }

            if (ModelState.IsValid)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, password);
                user.CreatedAt = DateTime.UtcNow; 
                context.Add(user);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,IsActive")] User user, string? newPassword)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userToUpdate = await context.User.FindAsync(id);
                    if (userToUpdate == null)
                    {
                        return NotFound();
                    }

                    context.Entry(userToUpdate).CurrentValues.SetValues(user); 

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        userToUpdate.PasswordHash = _passwordHasher.HashPassword(userToUpdate, newPassword);
                    }
                    
                    context.Update(userToUpdate);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await context.User.FindAsync(id);
            if (user != null)
            {
                context.User.Remove(user);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return context.User.Any(e => e.Id == id);
        }
    }
}
