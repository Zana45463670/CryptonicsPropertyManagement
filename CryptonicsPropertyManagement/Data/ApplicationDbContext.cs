using CryptonicsPropertyManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptonicsPropertyManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<PropertyManager> PropertyManagers => Set<PropertyManager>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<LeaseAgreement> LeaseAgreements => Set<LeaseAgreement>();
    public DbSet<Settlement> Settlements => Set<Settlement>();
    public DbSet<SystemUser> SystemUsers => Set<SystemUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Owner>(e =>
        {
            e.HasKey(x => x.OwnerId);
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.EmailAddress).HasMaxLength(200);
            e.Property(x => x.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<Property>(e =>
        {
            e.HasKey(x => x.PropertyId);
            e.Property(x => x.PropertyDescription).HasMaxLength(255);
            e.Property(x => x.PhysicalAddress).HasMaxLength(255).IsRequired();
            e.Property(x => x.MonthlyRent).HasPrecision(18, 2);
            e.HasIndex(x => x.PhysicalAddress).IsUnique();
            e.HasOne(x => x.Owner)
                .WithMany(o => o.Properties)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PropertyManager>(e =>
        {
            e.HasKey(x => x.ManagerId);
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.EmailAddress).HasMaxLength(200);
        });

        modelBuilder.Entity<Tenant>(e =>
        {
            e.HasKey(x => x.TenantId);
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.EmailAddress).HasMaxLength(200);
            e.Property(x => x.PassportIdNumber).HasMaxLength(50);
            e.Property(x => x.Nationality).HasMaxLength(100);
            e.Property(x => x.DeclaredMonthlyIncome).HasPrecision(18, 2);
            e.Property(x => x.VerificationStatus).HasMaxLength(20).HasDefaultValue("Pending");
        });

        modelBuilder.Entity<LeaseAgreement>(e =>
        {
            e.HasKey(x => x.LeaseId);
            e.Property(x => x.MonthlyRent).HasPrecision(18, 2);
            e.Property(x => x.LeaseStatus).HasMaxLength(20).HasDefaultValue("Active");
            e.HasOne(x => x.Property)
                .WithMany(p => p.LeaseAgreements)
                .HasForeignKey(x => x.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Tenant)
                .WithMany(t => t.LeaseAgreements)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Manager)
                .WithMany(m => m.LeaseAgreements)
                .HasForeignKey(x => x.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Settlement>(e =>
        {
            e.HasKey(x => x.SettlementId);
            e.Property(x => x.GrossRent).HasPrecision(18, 2);
            e.Property(x => x.MaintenanceCosts).HasPrecision(18, 2);
            e.Property(x => x.NetAmount).HasPrecision(18, 2);
            e.Property(x => x.ManagementFee).HasPrecision(18, 2);
            e.Property(x => x.OwnerPayout).HasPrecision(18, 2);
            e.Property(x => x.CryptoInvoiceLink).HasMaxLength(500);
            e.HasOne(x => x.Lease)
                .WithMany(l => l.Settlements)
                .HasForeignKey(x => x.LeaseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SystemUser>(e =>
        {
            e.HasKey(x => x.UserId);
            e.Property(x => x.Username).HasMaxLength(100).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired();
            e.Property(x => x.UserRole).HasMaxLength(50);
            e.HasIndex(x => x.Username).IsUnique();
        });
    }
}
