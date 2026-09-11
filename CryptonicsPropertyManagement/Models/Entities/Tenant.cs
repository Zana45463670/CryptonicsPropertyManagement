namespace CryptonicsPropertyManagement.Models.Entities;

public class Tenant
{
    public int TenantId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PassportIdNumber { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public decimal DeclaredMonthlyIncome { get; set; }
    public string VerificationStatus { get; set; } = "Pending";

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
}
