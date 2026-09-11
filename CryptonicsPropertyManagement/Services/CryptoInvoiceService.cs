using CryptonicsPropertyManagement.Services.Interfaces;

namespace CryptonicsPropertyManagement.Services;

/// <summary>
/// Generates a unique crypto payment link using a GUID.
/// In production this would call a real payment gateway API.
/// </summary>
public class CryptoInvoiceService : ICryptoInvoiceService
{
    public string GenerateInvoiceLink(int leaseId, decimal amount)
    {
        string uniqueId = Guid.NewGuid().ToString("N");
        return $"https://pay.cryptonics.app/invoice/{uniqueId}?lease={leaseId}&amount={amount:F2}";
    }
}
