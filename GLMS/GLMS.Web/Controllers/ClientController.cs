/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;
using GLMS.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _clients;

        public ClientController(IClientRepository clients) => _clients = clients;

        public async Task<IActionResult> Index() =>
            View(await _clients.GetAllWithContractsAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid) return View(client);
            await _clients.AddAsync(client);
            await _clients.SaveChangesAsync();
            TempData["Success"] = $"Client '{client.Name}' created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _clients.GetByIdWithContractsAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clients.GetByIdAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id) return BadRequest();
            if (!ModelState.IsValid) return View(client);

            _clients.Update(client);
            await _clients.SaveChangesAsync();
            TempData["Success"] = $"Client '{client.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clients.GetByIdWithContractsAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _clients.GetByIdAsync(id);
            if (client != null)
            {
                _clients.Remove(client);
                await _clients.SaveChangesAsync();
                TempData["Success"] = "Client deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
