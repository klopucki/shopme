using Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intranet.Controllers
{
    public class AuditController(ShopMeDbContext context) : Controller
    {
        // GET: Audit
        public async Task<IActionResult> Index()
        {
            var auditLogs = await context.AuditLog
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
            return View(auditLogs);
        }

        // GET: Audit/Details/5 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLog = await context.AuditLog
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auditLog == null)
            {
                return NotFound();
            }

            return View(auditLog);
        }
    }
}