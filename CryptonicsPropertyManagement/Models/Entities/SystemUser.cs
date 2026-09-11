namespace CryptonicsPropertyManagement.Models.Entities;

public class SystemUser
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string UserRole { get; set; } = "Clerk"; // Administrator / PropertyManager / Clerk
}
