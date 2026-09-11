using CryptonicsPropertyManagement.Models.Entities;

namespace CryptonicsPropertyManagement.Repositories.Interfaces;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync(int id);
    Task<Property> AddAsync(Property property);
    Task UpdateAsync(Property property);
    Task DeleteAsync(int id);
    Task<bool> AddressExistsAsync(string address, int? excludePropertyId = null);
}
