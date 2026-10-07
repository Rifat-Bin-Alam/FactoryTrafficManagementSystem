using FactoryTrafficManagement.Domain.Entities;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface ISensorEventRepository
{
    Task<SensorEvent?> GetByIdAsync(string eventId);

    Task AddAsync(SensorEvent sensorEvent);

    Task SaveChangesAsync();
}