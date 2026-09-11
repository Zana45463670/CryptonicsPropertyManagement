using CryptonicsPropertyManagement.Models.Entities;

namespace CryptonicsPropertyManagement.Repositories.Interfaces;

public interface ITenantRepository
{
    Task<IEnumerable<Tenant>> GetAllAsync();
    Task<Tenant?> GetByIdAsync(int id);
    Task<Tenant> AddAsync(Tenant tenant);
    Task UpdateAsync(Tenant tenant);
    Task DeleteAsync(int id);
    Task UpdateVerificationStatusAsync(int tenantId, string status);
}
