/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ClientCount = await _context.Clients.CountAsync();
            ViewBag.ContractCount = await _context.Contracts.CountAsync();
            ViewBag.ActiveContracts = await _context.Contracts.CountAsync(c => c.Status == "Active");
            ViewBag.ExpiredContracts = await _context.Contracts.CountAsync(c => c.Status == "Expired");
            ViewBag.ServiceRequestCount = await _context.ServiceRequests.CountAsync();
            ViewBag.PendingRequests = await _context.ServiceRequests.CountAsync(s => s.Status == "Pending");

            var recentRequests = await _context.ServiceRequests
                .Include(s => s.Contract)
                    .ThenInclude(c => c!.Client)
                .OrderByDescending(s => s.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(recentRequests);
        }
    }
}
