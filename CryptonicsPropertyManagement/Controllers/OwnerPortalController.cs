using System.Security.Claims;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptonicsPropertyManagement.Controllers;

[Authorize]
public class OwnerPortalController : Controller
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerPortalController(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    public async Task<IActionResult> Index()
    {
        var ownerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (ownerIdClaim == null || !int.TryParse(ownerIdClaim, out var ownerId))
        {
            return RedirectToAction("Login", "Account");
        }

        var owner = await _ownerRepository.GetByIdAsync(ownerId);
        if (owner == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(owner);
    }
}
