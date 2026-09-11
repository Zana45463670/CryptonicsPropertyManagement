using CryptonicsPropertyManagement.Data;
using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptonicsPropertyManagement.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly ApplicationDbContext _context;

    public PropertyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Properties
            .Include(p => p.Owner)
            .OrderBy(p => p.PhysicalAddress)
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync(int id)
    {
        return await _context.Properties
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.PropertyId == id);
    }

    public async Task<Property> AddAsync(Property property)
    {
        if (await AddressExistsAsync(property.PhysicalAddress))
            throw new InvalidOperationException("A property with this address already exists.");

        _context.Properties.Add(property);
        await _context.SaveChangesAsync();
        return property;
    }

    public async Task UpdateAsync(Property property)
    {
        if (await AddressExistsAsync(property.PhysicalAddress, property.PropertyId))
            throw new InvalidOperationException("A property with this address already exists.");

        _context.Properties.Update(property);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property != null)
        {
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> AddressExistsAsync(string address, int? excludePropertyId = null)
    {
        var query = _context.Properties.Where(p => p.PhysicalAddress == address);
        if (excludePropertyId.HasValue)
            query = query.Where(p => p.PropertyId != excludePropertyId.Value);
        return await query.AnyAsync();
    }
}
