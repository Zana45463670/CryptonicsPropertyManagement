using Microsoft.AspNetCore.Mvc;

namespace CryptonicsPropertyManagement.Controllers;

public class LandingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
