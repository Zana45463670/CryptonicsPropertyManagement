namespace CryptonicsPropertyManagement.Models.Entities;

public class Property
{
    public int PropertyId { get; set; }
    public string PropertyDescription { get; set; } = string.Empty;
    public string PhysicalAddress { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public decimal MonthlyRent { get; set; }
    public bool VacancyStatus { get; set; } = true; // true = Vacant

    public Owner? Owner { get; set; }
    public ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
}
