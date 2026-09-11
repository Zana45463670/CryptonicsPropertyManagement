using CryptonicsPropertyManagement.Data;
using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptonicsPropertyManagement.Repositories;

public class LeaseRepository : ILeaseRepository
{
    private readonly ApplicationDbContext _context;

    public LeaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LeaseAgreement>> GetAllAsync()
    {
        return await _context.LeaseAgreements
            .Include(l => l.Property)
            .Include(l => l.Tenant)
            .Include(l => l.Manager)
            .OrderByDescending(l => l.LeaseStartDate)
            .ToListAsync();
    }

    public async Task<LeaseAgreement?> GetByIdAsync(int id)
    {
        return await _context.LeaseAgreements
            .Include(l => l.Property)
            .Include(l => l.Tenant)
            .Include(l => l.Manager)
            .FirstOrDefaultAsync(l => l.LeaseId == id);
    }

    public async Task<LeaseAgreement> AddAsync(LeaseAgreement lease)
    {
        _context.LeaseAgreements.Add(lease);
        await _context.SaveChangesAsync();
        return lease;
    }

    public async Task UpdateAsync(LeaseAgreement lease)
    {
        _context.LeaseAgreements.Update(lease);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var lease = await _context.LeaseAgreements.FindAsync(id);
        if (lease != null)
        {
            _context.LeaseAgreements.Remove(lease);
            await _context.SaveChangesAsync();
        }
    }
}
