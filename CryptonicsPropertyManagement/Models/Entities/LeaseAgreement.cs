namespace CryptonicsPropertyManagement.Models.Entities;

public class LeaseAgreement
{
    public int LeaseId { get; set; }
    public int PropertyId { get; set; }
    public int TenantId { get; set; }
    public int ManagerId { get; set; }
    public DateTime LeaseStartDate { get; set; }
    public DateTime LeaseEndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public string LeaseStatus { get; set; } = "Active"; // Active / Expired / Terminated

    public Property? Property { get; set; }
    public Tenant? Tenant { get; set; }
    public PropertyManager? Manager { get; set; }
    public ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();
}
