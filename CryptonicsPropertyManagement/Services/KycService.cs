using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Services.Interfaces;

namespace CryptonicsPropertyManagement.Services;

/// <summary>
/// Simulated KYC verification. In production this would call an external verification service.
/// </summary>
public class KycService : IKycService
{
    private const decimal MinimumIncome = 5000m;

    public KycResult VerifyTenant(Tenant tenant)
    {
        bool hasPassport = !string.IsNullOrWhiteSpace(tenant.PassportIdNumber);
        bool hasNationality = !string.IsNullOrWhiteSpace(tenant.Nationality);
        bool meetsIncome = tenant.DeclaredMonthlyIncome >= MinimumIncome;

        bool approved = hasPassport && hasNationality && meetsIncome;

        return new KycResult
        {
            Status = approved ? "Approved" : "Declined",
            Message = approved
                ? "KYC verification passed. Tenant meets all requirements."
                : "KYC verification failed. Requirements not met (passport, nationality, and minimum income of R5,000 required)."
        };
    }
}
