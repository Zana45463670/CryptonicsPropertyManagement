namespace CryptonicsPropertyManagement.Models.Entities;

public class PropertyManager
{
    public int ManagerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
}
