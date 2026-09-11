namespace CryptonicsPropertyManagement.Services.Interfaces;

public class SettlementResult
{
    public decimal GrossRent { get; set; }
    public decimal MaintenanceCosts { get; set; }
    public decimal NetAmount { get; set; }
    public decimal ManagementFee { get; set; }
    public decimal OwnerPayout { get; set; }
    public int DaysOccupied { get; set; }
    public bool IsProRata => DaysOccupied < 30;
}

public interface ISettlementService
{
    SettlementResult Calculate(decimal monthlyRent, decimal maintenanceCosts, int daysOccupied);
}
