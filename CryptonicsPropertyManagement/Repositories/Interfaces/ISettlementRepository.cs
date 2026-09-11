using CryptonicsPropertyManagement.Models.Entities;

namespace CryptonicsPropertyManagement.Repositories.Interfaces;

public interface ISettlementRepository
{
    Task<IEnumerable<Settlement>> GetAllAsync();
    Task<Settlement?> GetByIdAsync(int id);
    Task<Settlement> AddAsync(Settlement settlement);
    Task UpdateAsync(Settlement settlement);
    Task DeleteAsync(int id);
}
