/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;
using GLMS.Web.Patterns.Strategy;
using GLMS.Web.Repositories;
using GLMS.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Web.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestRepository _requests;
        private readonly IContractRepository _contracts;
        private readonly ICurrencyService _currencyService;
        private readonly IValidationStrategySelector _validator;

        public ServiceRequestController(
            IServiceRequestRepository requests,
            IContractRepository contracts,
            ICurrencyService currencyService,
            IValidationStrategySelector validator)
        {
            _requests = requests;
            _contracts = contracts;
            _currencyService = currencyService;
            _validator = validator;
        }

        public async Task<IActionResult> Index() =>
            View(await _requests.GetAllWithRelationsAsync());

        public async Task<IActionResult> Details(int id)
        {
            var request = await _requests.GetDetailsAsync(id);
            if (request == null) return NotFound();
            return View(request);
        }

        public async Task<IActionResult> Create(int? contractId)
        {
            ViewBag.ExchangeRate = await _currencyService.GetUsdToZarRateAsync();

            if (contractId.HasValue)
            {
                var contract = await _contracts.GetDetailsAsync(contractId.Value);
                if (contract != null && !WorkflowValidator.CanCreateServiceRequest(contract))
                {
                    TempData["Error"] = WorkflowValidator.GetBlockedReason(contract);
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.PreselectedContractId = contractId.Value;
            }

            await PopulateContractsDropdown(contractId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            var contract = await _contracts.GetByIdAsync(request.ContractId);

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Selected contract does not exist.");
            }
            else
            {
                //Strategy pattern
                var result = _validator.Validate(contract, request);
                if (!result.IsValid)
                    ModelState.AddModelError("ContractId", result.ErrorMessage ?? "Request is not valid for this contract.");
            }

            ModelState.Remove("Contract");

            if (!ModelState.IsValid)
            {
                ViewBag.ExchangeRate = await _currencyService.GetUsdToZarRateAsync();
                await PopulateContractsDropdown(request.ContractId);
                return View(request);
            }

            request.ExchangeRate = await _currencyService.GetUsdToZarRateAsync();
            request.CostZAR = CurrencyCalculator.ConvertUsdToZar(request.CostUSD, request.ExchangeRate);
            request.CreatedAt = DateTime.UtcNow;

            await _requests.AddAsync(request);
            await _requests.SaveChangesAsync();
            TempData["Success"] = "Service request created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var request = await _requests.GetDetailsAsync(id);
            if (request == null) return NotFound();
            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _requests.GetByIdAsync(id);
            if (request != null)
            {
                _requests.Remove(request);
                await _requests.SaveChangesAsync();
                TempData["Success"] = "Service request deleted.";
            }
            return RedirectToAction(nameof(Index));
        }

        //AJAX endpoint used by the Create view to refresh the live rate.
        [HttpGet]
        public async Task<IActionResult> GetRate()
        {
            var rate = await _currencyService.GetUsdToZarRateAsync();
            return Json(new { success = true, rate });
        }

        private async Task PopulateContractsDropdown(int? selectedId = null)
        {
            var activeContracts = await _contracts.GetActiveForDropdownAsync();

            ViewBag.Contracts = new SelectList(
                activeContracts.Select(c => new { c.Id, Label = $"{c.Client!.Name} — {c.ServiceLevel} [{c.Status}]" }),
                "Id", "Label", selectedId);
        }
    }
}
