using CryptonicsPropertyManagement.Data;
using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptonicsPropertyManagement.Repositories;

public class PropertyManagerRepository : IPropertyManagerRepository
{
    private readonly ApplicationDbContext _context;

    public PropertyManagerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PropertyManager>> GetAllAsync()
    {
        return await _context.PropertyManagers
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToListAsync();
    }

    public async Task<PropertyManager?> GetByIdAsync(int id)
    {
        return await _context.PropertyManagers.FindAsync(id);
    }

    public async Task<PropertyManager> AddAsync(PropertyManager manager)
    {
        _context.PropertyManagers.Add(manager);
        await _context.SaveChangesAsync();
        return manager;
    }

    public async Task UpdateAsync(PropertyManager manager)
    {
        _context.PropertyManagers.Update(manager);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var manager = await _context.PropertyManagers.FindAsync(id);
        if (manager != null)
        {
            _context.PropertyManagers.Remove(manager);
            await _context.SaveChangesAsync();
        }
    }
}
