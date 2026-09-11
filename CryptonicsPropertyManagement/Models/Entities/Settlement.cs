namespace CryptonicsPropertyManagement.Models.Entities;

public class Settlement
{
    public int SettlementId { get; set; }
    public int LeaseId { get; set; }
    public decimal GrossRent { get; set; }
    public decimal MaintenanceCosts { get; set; }
    public decimal NetAmount { get; set; }
    public decimal ManagementFee { get; set; }
    public decimal OwnerPayout { get; set; }
    public int DaysOccupied { get; set; }
    public DateTime SettlementDate { get; set; }
    public string CryptoInvoiceLink { get; set; } = string.Empty;

    public LeaseAgreement? Lease { get; set; }
}
