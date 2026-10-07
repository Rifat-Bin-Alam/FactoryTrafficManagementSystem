using FactoryTrafficManagement.Domain.Entities;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(string vehicleId);

    Task<IReadOnlyList<Vehicle>> GetQueuedByJunctionIdAsync(
        string junctionId);

    Task AddAsync(Vehicle vehicle);

    Task SaveChangesAsync();
}