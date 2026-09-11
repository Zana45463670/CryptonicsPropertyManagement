# Cryptonics Property Management System

**.NET 8 · ASP.NET Core MVC · EF Core + SQLite (temporary) · Interfaces + Repositories**

Modernised version of the original Cryptonics / AtlasPremierProperties developer guide (VS 2022 · .NET Framework 4.8 · MS Access).

## What’s included

| Feature | Status |
|---------|--------|
| CRUD – Properties, Owners, Tenants | Real |
| Settlement calculations (pro-rata, 12% fee) | Real |
| KYC verification button + status update | Simulated (internal rules) |
| Unique crypto invoice link (GUID) | Simulated |
| Co-Host performance report + Chart.js | Real |
| Database | SQLite file (`cryptonics.db`) – easy to swap later |

## Architecture (best practices)

```
Controllers  →  Services (interfaces)  →  Repositories (interfaces)  →  EF Core DbContext  →  SQLite
```

- **Dependency Injection** for all repositories and services
- **Interfaces** for every repository and service
- **Async** data access
- **Seed data** on first run
- Clear separation of concerns

## Settlement formula (from original guide)

```
Effective Rent (pro-rata) = (Monthly Rent ÷ 30) × Days Occupied
Net Amount               = Effective Rent − Maintenance Costs
Management Fee           = Net Amount × 12%
Owner Payout             = Net Amount − Management Fee
```

Example: R10 000 rent, R500 maintenance, 30 days → Owner payout **R8 360**.

## Quick start

```bash
cd CryptonicsPropertyManagement
dotnet run
```

Open https://localhost:5xxx (or the URL shown in the console).

SQLite database file `cryptonics.db` is created automatically on first run and seeded with sample owners, properties, tenants and one lease.

## Project structure

```
CryptonicsPropertyManagement/
├── Controllers/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Models/Entities/
├── Repositories/ (+ Interfaces/)
├── Services/ (+ Interfaces/)
└── Views/
```

## Scaling later

When you are ready to move off the temporary database:

1. Change the connection string in `appsettings.json`
2. Swap `UseSqlite` for `UseSqlServer` / `UseNpgsql` in `Program.cs`
3. Run EF migrations (`dotnet ef migrations add Initial` + `dotnet ef database update`)

No repository or service code needs to change.

## Original guide mapping

| Original (Access / Framework 4.8) | This project (.NET 8) |
|-----------------------------------|-----------------------|
| OleDb + `?` parameters            | EF Core LINQ          |
| Hard-coded `C:\CryptoniCS\...`    | SQLite file / config  |
| Manual repositories               | Interface + DI        |
| Simulated KYC / Crypto            | Same (interfaces ready for real APIs) |
| Chart.js report                   | Preserved             |

Deadline reference from original guide: **31 August 2026**.
