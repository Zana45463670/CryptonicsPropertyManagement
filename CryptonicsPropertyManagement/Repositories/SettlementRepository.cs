using CryptonicsPropertyManagement.Data;
using CryptonicsPropertyManagement.Models.Entities;
using CryptonicsPropertyManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptonicsPropertyManagement.Repositories;

public class SettlementRepository : ISettlementRepository
{
    private readonly ApplicationDbContext _context;

    public SettlementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Settlement>> GetAllAsync()
    {
        return await _context.Settlements
            .Include(s => s.Lease)
                .ThenInclude(l => l!.Property)
            .Include(s => s.Lease)
                .ThenInclude(l => l!.Tenant)
            .OrderByDescending(s => s.SettlementDate)
            .ToListAsync();
    }

    public async Task<Settlement?> GetByIdAsync(int id)
    {
        return await _context.Settlements
            .Include(s => s.Lease)
                .ThenInclude(l => l!.Property)
            .FirstOrDefaultAsync(s => s.SettlementId == id);
    }

    public async Task<Settlement> AddAsync(Settlement settlement)
    {
        _context.Settlements.Add(settlement);
        await _context.SaveChangesAsync();
        return settlement;
    }

    public async Task UpdateAsync(Settlement settlement)
    {
        _context.Settlements.Update(settlement);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var settlement = await _context.Settlements.FindAsync(id);
        if (settlement != null)
        {
            _context.Settlements.Remove(settlement);
            await _context.SaveChangesAsync();
        }
    }
}
