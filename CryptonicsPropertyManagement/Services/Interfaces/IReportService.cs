namespace CryptonicsPropertyManagement.Services.Interfaces;

public class CoHostStats
{
    public string ManagerName { get; set; } = string.Empty;
    public int TotalDays { get; set; }
    public int DaysOccupied { get; set; }

    public decimal OccupancyRate =>
        TotalDays > 0
            ? Math.Round((decimal)DaysOccupied / TotalDays * 100, 1)
            : 0;
}

public interface IReportService
{
    Task<IEnumerable<CoHostStats>> GetCoHostPerformanceAsync(DateTime start, DateTime end);
}
