using FactoryTrafficManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactoryTrafficManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Junction> Junctions => Set<Junction>();

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    public DbSet<SensorEvent> SensorEvents => Set<SensorEvent>();

    public DbSet<ControllerCommand> ControllerCommands => Set<ControllerCommand>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}