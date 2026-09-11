namespace CryptonicsPropertyManagement.Services.Interfaces;

public interface ICryptoInvoiceService
{
    string GenerateInvoiceLink(int leaseId, decimal amount);
}
