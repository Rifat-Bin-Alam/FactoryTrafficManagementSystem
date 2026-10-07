using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FactoryTrafficManagement.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;

    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetByIdAsync(string vehicleId)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(x => x.Id == vehicleId);
    }

    public async Task<IReadOnlyList<Vehicle>> GetQueuedByJunctionIdAsync(
        string junctionId)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .Where(x =>
                x.JunctionId == junctionId &&
                x.IsQueued)
            .OrderBy(x => x.ArrivedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}