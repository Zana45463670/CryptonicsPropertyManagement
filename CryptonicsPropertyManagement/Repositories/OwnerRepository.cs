using CryptonicsPropertyManagement.Data;
using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptonicsPropertyManagement.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly ApplicationDbContext _context;

    public OwnerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Owner>> GetAllAsync()
    {
        return await _context.Owners
            .OrderBy(o => o.LastName)
            .ThenBy(o => o.FirstName)
            .ToListAsync();
    }

    public async Task<Owner?> GetByIdAsync(int id)
    {
        return await _context.Owners.FindAsync(id);
    }

    public async Task<Owner> AddAsync(Owner owner)
    {
        _context.Owners.Add(owner);
        await _context.SaveChangesAsync();
        return owner;
    }

    public async Task UpdateAsync(Owner owner)
    {
        _context.Owners.Update(owner);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var owner = await _context.Owners.FindAsync(id);
        if (owner != null)
        {
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
        }
    }
}
