using CryptonicsPropertyManagement.Models.Entities;

namespace CryptonicsPropertyManagement.Repositories.Interfaces;

public interface IPropertyManagerRepository
{
    Task<IEnumerable<PropertyManager>> GetAllAsync();
    Task<PropertyManager?> GetByIdAsync(int id);
    Task<PropertyManager> AddAsync(PropertyManager manager);
    Task UpdateAsync(PropertyManager manager);
    Task DeleteAsync(int id);
}
