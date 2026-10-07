using FactoryTrafficManagement.Application.DTOs;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface ISensorEventService
{
    Task ProcessAsync(SensorEventRequest request);
}