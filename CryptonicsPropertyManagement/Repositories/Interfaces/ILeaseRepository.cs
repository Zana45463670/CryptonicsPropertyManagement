using CryptonicsPropertyManagement.Models.Entities;

namespace CryptonicsPropertyManagement.Repositories.Interfaces;

public interface ILeaseRepository
{
    Task<IEnumerable<LeaseAgreement>> GetAllAsync();
    Task<LeaseAgreement?> GetByIdAsync(int id);
    Task<LeaseAgreement> AddAsync(LeaseAgreement lease);
    Task UpdateAsync(LeaseAgreement lease);
    Task DeleteAsync(int id);
}
