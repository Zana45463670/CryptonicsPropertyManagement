using System.Security.Claims;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CryptonicsPropertyManagement.Controllers;

public class AccountController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly PasswordHasher<object> _passwordHasher = new();

    public AccountController(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        var owner = await _ownerRepository.GetByEmailAsync(email ?? string.Empty);

        if (owner == null || string.IsNullOrEmpty(owner.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        var result = _passwordHasher.VerifyHashedPassword(null!, owner.PasswordHash, password ?? string.Empty);
        if (result == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, owner.OwnerId.ToString()),
            new(ClaimTypes.Name, owner.FullName),
            new(ClaimTypes.Email, owner.EmailAddress)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "OwnerPortal");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
