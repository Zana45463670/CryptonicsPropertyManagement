using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using CryptonicsPropertyManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptonicsPropertyManagement.Controllers;

public class SettlementsController : Controller
{
    private readonly ISettlementRepository _settlementRepo;
    private readonly ILeaseRepository _leaseRepo;
    private readonly ISettlementService _settlementService;
    private readonly ICryptoInvoiceService _cryptoService;

    public SettlementsController(
        ISettlementRepository settlementRepo,
        ILeaseRepository leaseRepo,
        ISettlementService settlementService,
        ICryptoInvoiceService cryptoService)
    {
        _settlementRepo = settlementRepo;
        _leaseRepo = leaseRepo;
        _settlementService = settlementService;
        _cryptoService = cryptoService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _settlementRepo.GetAllAsync());
    }

    public async Task<IActionResult> Create()
    {
        await PopulateLeasesDropDown();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int leaseId, decimal maintenanceCosts, int daysOccupied)
    {
        try
        {
            var lease = await _leaseRepo.GetByIdAsync(leaseId);
            if (lease == null)
            {
                ModelState.AddModelError("", "Lease not found.");
                await PopulateLeasesDropDown();
                return View();
            }

            var result = _settlementService.Calculate(lease.MonthlyRent, maintenanceCosts, daysOccupied);
            string invoiceLink = _cryptoService.GenerateInvoiceLink(leaseId, result.OwnerPayout);

            var settlement = new Settlement
            {
                LeaseId = leaseId,
                GrossRent = result.GrossRent,
                MaintenanceCosts = result.MaintenanceCosts,
                NetAmount = result.NetAmount,
                ManagementFee = result.ManagementFee,
                OwnerPayout = result.OwnerPayout,
                DaysOccupied = result.DaysOccupied,
                SettlementDate = DateTime.UtcNow,
                CryptoInvoiceLink = invoiceLink
            };

            var created = await _settlementRepo.AddAsync(settlement);
            TempData["Success"] = $"Settlement ID {created.SettlementId} created successfully.";
            TempData["InvoiceLink"] = invoiceLink;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateLeasesDropDown();
            return View();
        }
    }

    private async Task PopulateLeasesDropDown()
    {
        var leases = await _leaseRepo.GetAllAsync();
        var items = leases.Select(l => new
        {
            l.LeaseId,
            Display = $"#{l.LeaseId} - {l.Property?.PhysicalAddress ?? "N/A"} ({l.Tenant?.FullName ?? "N/A"})"
        });
        ViewBag.Leases = new SelectList(items, "LeaseId", "Display");
    }
}
