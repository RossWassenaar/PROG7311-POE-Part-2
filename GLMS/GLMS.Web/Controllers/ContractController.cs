/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

//Title: Handle Requests with Controllers in ASP.NET Core MVC
//Author: Microsoft
//Date: 20 April 2026
//Version: .NET 10
//Availability: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions


using GLMS.Web.Models;
using GLMS.Web.Patterns.Factory;
using GLMS.Web.Patterns.Observer;
using GLMS.Web.Repositories;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractRepository _contracts;
        private readonly IClientRepository _clients;
        private readonly IFileService _fileService;
        private readonly IContractFactoryProvider _factoryProvider;
        private readonly IContractStatusPublisher _statusPublisher;
        private readonly IWebHostEnvironment _env;

        public ContractController(
            IContractRepository contracts,
            IClientRepository clients,
            IFileService fileService,
            IContractFactoryProvider factoryProvider,
            IContractStatusPublisher statusPublisher,
            IWebHostEnvironment env)
        {
            _contracts = contracts;
            _clients = clients;
            _fileService = fileService;
            _factoryProvider = factoryProvider;
            _statusPublisher = statusPublisher;
            _env = env;
        }

        //LINQ search/filter by Status and Date Range
        public async Task<IActionResult> Index(string? status, DateTime? startFrom, DateTime? startTo)
        {
            ViewBag.StatusFilter = status;
            ViewBag.StartFrom = startFrom?.ToString("yyyy-MM-dd");
            ViewBag.StartTo = startTo?.ToString("yyyy-MM-dd");

            var contracts = await _contracts.SearchAsync(status, startFrom, startTo);
            return View(contracts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var contract = await _contracts.GetDetailsAsync(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract input, IFormFile? signedAgreement)
        {
            if (!ModelState.IsValid) { await PopulateDropdowns(); return View(input); }

            Contract contract;
            try
            {
                //Factory Pattern
                var factory = _factoryProvider.GetFactory(input.ServiceLevel);
                contract = factory.Create(input.ClientId, input.StartDate, input.EndDate);
                contract.Status = input.Status;
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDropdowns();
                return View(input);
            }

            if (signedAgreement != null)
            {
                try
                {
                    _fileService.ValidateFile(signedAgreement);
                    var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "agreements");
                    contract.SignedAgreementPath = await _fileService.SaveFileAsync(signedAgreement, uploadsPath);
                    contract.SignedAgreementFileName = signedAgreement.FileName;
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("signedAgreement", ex.Message);
                    await PopulateDropdowns();
                    return View(input);
                }
            }

            await _contracts.AddAsync(contract);
            await _contracts.SaveChangesAsync();
            TempData["Success"] = $"Contract created ({contract.ServiceLevel}).";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _contracts.GetByIdAsync(id);
            if (contract == null) return NotFound();
            await PopulateDropdowns();
            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract, IFormFile? signedAgreement)
        {
            if (id != contract.Id) return BadRequest();
            if (!ModelState.IsValid) { await PopulateDropdowns(); return View(contract); }

            var existing = await _contracts.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var oldStatus = existing.Status;

            existing.ClientId = contract.ClientId;
            existing.StartDate = contract.StartDate;
            existing.EndDate = contract.EndDate;
            existing.Status = contract.Status;
            existing.ServiceLevel = contract.ServiceLevel;

            if (signedAgreement != null)
            {
                try
                {
                    _fileService.ValidateFile(signedAgreement);
                    var uploadsPath = Path.Combine(_env.WebRootPath, "uploads", "agreements");
                    existing.SignedAgreementPath = await _fileService.SaveFileAsync(signedAgreement, uploadsPath);
                    existing.SignedAgreementFileName = signedAgreement.FileName;
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("signedAgreement", ex.Message);
                    await PopulateDropdowns();
                    return View(contract);
                }
            }

            await _contracts.SaveChangesAsync();

            //Observer pattern
            if (!string.Equals(oldStatus, existing.Status, StringComparison.OrdinalIgnoreCase))
            {
                var client = await _clients.GetByIdAsync(existing.ClientId);
                await _statusPublisher.PublishAsync(new ContractStatusChangedEvent(
                    existing.Id,
                    client?.Name,
                    oldStatus,
                    existing.Status,
                    DateTime.UtcNow));
            }

            TempData["Success"] = "Contract updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _contracts.GetDetailsAsync(id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _contracts.GetByIdAsync(id);
            if (contract != null)
            {
                _contracts.Remove(contract);
                await _contracts.SaveChangesAsync();
                TempData["Success"] = "Contract deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DownloadAgreement(int id)
        {
            var contract = await _contracts.GetByIdAsync(id);
            if (contract == null || string.IsNullOrEmpty(contract.SignedAgreementPath))
                return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, "uploads", "agreements", contract.SignedAgreementPath);
            if (!System.IO.File.Exists(filePath)) return NotFound();

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, "application/pdf", contract.SignedAgreementFileName ?? "agreement.pdf");
        }

        private async Task PopulateDropdowns()
        {
            var clients = await _clients.GetAllWithContractsAsync();
            ViewBag.Clients = new SelectList(clients, "Id", "Name");
            ViewBag.Statuses = new SelectList(new[] { "Draft", "Active", "Expired", "On Hold" });
            ViewBag.ServiceLevels = new SelectList(new[] { "Standard", "Premium", "Enterprise" });
        }
    }
}
