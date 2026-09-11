using CryptonicsPropertyManagement.Services.Interfaces;

namespace CryptonicsPropertyManagement.Services;

/// <summary>
/// Settlement formula (stakeholder interview 20 March 2026):
/// Net Amount = Gross Rent − Maintenance Costs
/// Management Fee = Net Amount × 12%
/// Owner Payout = Net Amount − Management Fee
/// Pro-rata = (Monthly Rent ÷ 30) × Days Occupied
/// </summary>
public class SettlementService : ISettlementService
{
    private const decimal FeeRate = 0.12m;
    private const int DaysPerMonth = 30;

    public SettlementResult Calculate(decimal monthlyRent, decimal maintenanceCosts, int daysOccupied)
    {
        if (daysOccupied < 1 || daysOccupied > 31)
            throw new ArgumentException("Days occupied must be between 1 and 31.");

        if (maintenanceCosts < 0)
            throw new ArgumentException("Maintenance costs cannot be negative.");

        // Step 1: Pro-rata (if daysOccupied = 30, result equals full rent)
        decimal effectiveRent = Math.Round((monthlyRent / DaysPerMonth) * daysOccupied, 2);

        // Step 2: Deduct maintenance BEFORE management fee
        decimal netAmount = effectiveRent - maintenanceCosts;

        if (netAmount < 0)
            throw new InvalidOperationException("Maintenance costs exceed the rent amount.");

        // Step 3: 12% management fee on net amount
        decimal managementFee = Math.Round(netAmount * FeeRate, 2);

        // Step 4: Owner receives the remainder
        decimal ownerPayout = Math.Round(netAmount - managementFee, 2);

        return new SettlementResult
        {
            GrossRent = effectiveRent,
            MaintenanceCosts = maintenanceCosts,
            NetAmount = Math.Round(netAmount, 2),
            ManagementFee = managementFee,
            OwnerPayout = ownerPayout,
            DaysOccupied = daysOccupied
        };
    }
}
