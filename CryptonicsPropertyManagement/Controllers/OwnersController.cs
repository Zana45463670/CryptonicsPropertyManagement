using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptonicsPropertyManagement.Controllers;

public class OwnersController : Controller
{
    private readonly IOwnerRepository _ownerRepo;

    public OwnersController(IOwnerRepository ownerRepo)
    {
        _ownerRepo = ownerRepo;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _ownerRepo.GetAllAsync());
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Owner owner)
    {
        if (!ModelState.IsValid) return View(owner);

        await _ownerRepo.AddAsync(owner);
        TempData["Success"] = "Owner successfully added.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var owner = await _ownerRepo.GetByIdAsync(id);
        if (owner == null) return NotFound();
        return View(owner);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Owner owner)
    {
        if (id != owner.OwnerId) return NotFound();
        if (!ModelState.IsValid) return View(owner);

        await _ownerRepo.UpdateAsync(owner);
        TempData["Success"] = "Owner successfully updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var owner = await _ownerRepo.GetByIdAsync(id);
        if (owner == null) return NotFound();
        return View(owner);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _ownerRepo.DeleteAsync(id);
        TempData["Success"] = "Owner successfully deleted.";
        return RedirectToAction(nameof(Index));
    }
}
