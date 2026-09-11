using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CryptonicsPropertyManagement.Controllers;

public class PropertiesController : Controller
{
    private readonly IPropertyRepository _propertyRepo;
    private readonly IOwnerRepository _ownerRepo;

    public PropertiesController(IPropertyRepository propertyRepo, IOwnerRepository ownerRepo)
    {
        _propertyRepo = propertyRepo;
        _ownerRepo = ownerRepo;
    }

    public async Task<IActionResult> Index()
    {
        var properties = await _propertyRepo.GetAllAsync();
        return View(properties);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateOwnersDropDown();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Property property)
    {
        if (!ModelState.IsValid)
        {
            await PopulateOwnersDropDown();
            return View(property);
        }

        try
        {
            var created = await _propertyRepo.AddAsync(property);
            TempData["Success"] = $"Property successfully added. Property ID: {created.PropertyId}";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateOwnersDropDown();
            return View(property);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var property = await _propertyRepo.GetByIdAsync(id);
        if (property == null) return NotFound();

        await PopulateOwnersDropDown(property.OwnerId);
        return View(property);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Property property)
    {
        if (id != property.PropertyId) return NotFound();

        if (!ModelState.IsValid)
        {
            await PopulateOwnersDropDown(property.OwnerId);
            return View(property);
        }

        try
        {
            await _propertyRepo.UpdateAsync(property);
            TempData["Success"] = $"Property ID {property.PropertyId} successfully updated.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            await PopulateOwnersDropDown(property.OwnerId);
            return View(property);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var property = await _propertyRepo.GetByIdAsync(id);
        if (property == null) return NotFound();
        return View(property);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _propertyRepo.DeleteAsync(id);
        TempData["Success"] = $"Property ID {id} successfully deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateOwnersDropDown(int? selectedId = null)
    {
        var owners = await _ownerRepo.GetAllAsync();
        ViewBag.Owners = new SelectList(owners, "OwnerId", "FullName", selectedId);
    }
}
