using CryptonicsPropertyManagement.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace CryptonicsPropertyManagement.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Owners.Any()) return; // already seeded

        var hasher = new PasswordHasher<object>();

        var owners = new[]
        {
            new Owner { FirstName = "Thabo", LastName = "Molefe", EmailAddress = "thabo@example.com", PhoneNumber = "0821112233", PasswordHash = hasher.HashPassword(null!, "Password123") },
            new Owner { FirstName = "Sarah", LastName = "van der Berg", EmailAddress = "sarah@example.com", PhoneNumber = "0834445566", PasswordHash = hasher.HashPassword(null!, "Password123") }
        };
        context.Owners.AddRange(owners);
        context.SaveChanges();

        var managers = new[]
        {
            new PropertyManager { FirstName = "Lerato", LastName = "Nkosi", EmailAddress = "lerato@cryptonics.app" },
            new PropertyManager { FirstName = "Johan", LastName = "Botha", EmailAddress = "johan@cryptonics.app" }
        };
        context.PropertyManagers.AddRange(managers);
        context.SaveChanges();

        var properties = new[]
        {
            new Property
            {
                PropertyDescription = "2-bed apartment in Sandton",
                PhysicalAddress = "12 Rivonia Rd, Sandton",
                OwnerId = owners[0].OwnerId,
                MonthlyRent = 15000m,
                VacancyStatus = false
            },
            new Property
            {
                PropertyDescription = "Studio in Cape Town CBD",
                PhysicalAddress = "45 Long Street, Cape Town",
                OwnerId = owners[1].OwnerId,
                MonthlyRent = 9500m,
                VacancyStatus = true
            }
        };
        context.Properties.AddRange(properties);
        context.SaveChanges();

        var tenants = new[]
        {
            new Tenant
            {
                FirstName = "Aisha",
                LastName = "Patel",
                EmailAddress = "aisha@example.com",
                PassportIdNumber = "A12345678",
                Nationality = "South African",
                DeclaredMonthlyIncome = 25000m,
                VerificationStatus = "Pending"
            },
            new Tenant
            {
                FirstName = "Michael",
                LastName = "Chen",
                EmailAddress = "michael@example.com",
                PassportIdNumber = "",
                Nationality = "Chinese",
                DeclaredMonthlyIncome = 4000m,
                VerificationStatus = "Pending"
            }
        };
        context.Tenants.AddRange(tenants);
        context.SaveChanges();

        var leases = new[]
        {
            new LeaseAgreement
            {
                PropertyId = properties[0].PropertyId,
                TenantId = tenants[0].TenantId,
                ManagerId = managers[0].ManagerId,
                LeaseStartDate = DateTime.Today.AddMonths(-2),
                LeaseEndDate = DateTime.Today.AddMonths(10),
                MonthlyRent = 15000m,
                LeaseStatus = "Active"
            }
        };
        context.LeaseAgreements.AddRange(leases);
        context.SaveChanges();
    }
}