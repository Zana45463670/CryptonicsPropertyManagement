namespace CryptonicsPropertyManagement.Models.Entities;

public class Owner
{
    public int OwnerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
