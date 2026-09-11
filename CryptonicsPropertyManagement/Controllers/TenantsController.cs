using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using CryptonicsPropertyManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptonicsPropertyManagement.Controllers;

public class TenantsController : Controller
{
    private readonly ITenantRepository _tenantRepo;
    private readonly IKycService _kycService;

    public TenantsController(ITenantRepository tenantRepo, IKycService kycService)
    {
        _tenantRepo = tenantRepo;
        _kycService = kycService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _tenantRepo.GetAllAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tenant tenant)
    {
        if (!ModelState.IsValid) return View(tenant);

        tenant.VerificationStatus = "Pending";
        await _tenantRepo.AddAsync(tenant);
        TempData["Success"] = "Tenant successfully added.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var tenant = await _tenantRepo.GetByIdAsync(id);
        if (tenant == null) return NotFound();
        return View(tenant);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tenant tenant)
    {
        if (id != tenant.TenantId) return NotFound();
        if (!ModelState.IsValid) return View(tenant);

        await _tenantRepo.UpdateAsync(tenant);
        TempData["Success"] = "Tenant successfully updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var tenant = await _tenantRepo.GetByIdAsync(id);
        if (tenant == null) return NotFound();
        return View(tenant);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _tenantRepo.DeleteAsync(id);
        TempData["Success"] = "Tenant successfully deleted.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RunKyc(int id)
    {
        var tenant = await _tenantRepo.GetByIdAsync(id);
        if (tenant == null) return NotFound();

        var result = _kycService.VerifyTenant(tenant);
        await _tenantRepo.UpdateVerificationStatusAsync(id, result.Status);

        TempData["KycResult"] = result.Status;
        TempData["KycMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }
}
