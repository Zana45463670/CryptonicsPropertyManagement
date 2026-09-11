using CryptonicsPropertyManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CryptonicsPropertyManagement.Controllers;

public class ReportsController : Controller
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    public IActionResult CoHostPerformance() => View();

    [HttpPost]
    public async Task<IActionResult> CoHostPerformance(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
        {
            ModelState.AddModelError("", "End date must be after start date.");
            return View();
        }

        var data = (await _reportService.GetCoHostPerformanceAsync(startDate, endDate)).ToList();

        var labels = new List<string>();
        var rates = new List<decimal>();
        var colors = new List<string>();

        foreach (var item in data)
        {
            labels.Add(item.ManagerName);
            rates.Add(item.OccupancyRate);
            colors.Add(item.OccupancyRate >= 70 ? "#28a745" : "#dc3545");
        }

        ViewBag.ChartJson = JsonConvert.SerializeObject(new { labels, rates, colors });
        ViewBag.StartDate = startDate.ToString("dd MMM yyyy");
        ViewBag.EndDate = endDate.ToString("dd MMM yyyy");

        return View(data);
    }
}
