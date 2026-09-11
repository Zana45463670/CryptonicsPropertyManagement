using CryptonicsPropertyManagement.Models.Entities;

namespace CryptonicsPropertyManagement.Services.Interfaces;

public class KycResult
{
    public string Status { get; set; } = string.Empty; // "Approved" or "Declined"
    public string Message { get; set; } = string.Empty;
}

public interface IKycService
{
    KycResult VerifyTenant(Tenant tenant);
}
