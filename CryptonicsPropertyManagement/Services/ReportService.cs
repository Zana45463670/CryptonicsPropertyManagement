using CryptonicsPropertyManagement.Data;
using CryptonicsPropertyManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptonicsPropertyManagement.Services;

public class ReportService : IReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CoHostStats>> GetCoHostPerformanceAsync(DateTime start, DateTime end)
    {
        int totalDays = (end.Date - start.Date).Days + 1;

        // Count leases per manager that overlap the date range
        var leaseCounts = await _context.LeaseAgreements
            .Include(l => l.Manager)
            .Where(l => l.LeaseStartDate <= end && l.LeaseEndDate >= start)
            .GroupBy(l => new { l.ManagerId, Name = l.Manager!.FirstName + " " + l.Manager.LastName })
            .Select(g => new
            {
                ManagerName = g.Key.Name,
                LeaseCount = g.Count()
            })
            .ToListAsync();

        // Approximate days occupied as lease-count * totalDays (simple model matching original guide intent)
        // A more precise version would calculate actual overlapping days per lease.
        var stats = leaseCounts.Select(x => new CoHostStats
        {
            ManagerName = x.ManagerName,
            TotalDays = totalDays,
            DaysOccupied = Math.Min(x.LeaseCount * totalDays, totalDays) // clamp for demo
        }).ToList();

        // Better approximation: sum actual overlapping days
        var precise = new List<CoHostStats>();
        var managers = await _context.PropertyManagers.ToListAsync();
        foreach (var mgr in managers)
        {
            var leases = await _context.LeaseAgreements
                .Where(l => l.ManagerId == mgr.ManagerId
                            && l.LeaseStartDate <= end
                            && l.LeaseEndDate >= start)
                .ToListAsync();

            int occupied = 0;
            foreach (var lease in leases)
            {
                var overlapStart = lease.LeaseStartDate > start ? lease.LeaseStartDate : start;
                var overlapEnd = lease.LeaseEndDate < end ? lease.LeaseEndDate : end;
                occupied += (overlapEnd.Date - overlapStart.Date).Days + 1;
            }

            if (leases.Count > 0 || occupied > 0)
            {
                precise.Add(new CoHostStats
                {
                    ManagerName = mgr.FullName,
                    TotalDays = totalDays,
                    DaysOccupied = Math.Min(occupied, totalDays)
                });
            }
        }

        return precise.OrderByDescending(s => s.OccupancyRate);
    }
}
