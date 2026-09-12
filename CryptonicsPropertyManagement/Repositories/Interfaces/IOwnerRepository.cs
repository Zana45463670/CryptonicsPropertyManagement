using CryptonicsPropertyManagement.Models.Entities;

namespace CryptonicsPropertyManagement.Repositories.Interfaces;

public interface IOwnerRepository
{
    Task<IEnumerable<Owner>> GetAllAsync();
    Task<Owner?> GetByIdAsync(int id);
    Task<Owner?> GetByEmailAsync(string email);
    Task<Owner> AddAsync(Owner owner);
    Task UpdateAsync(Owner owner);
    Task DeleteAsync(int id);
}
