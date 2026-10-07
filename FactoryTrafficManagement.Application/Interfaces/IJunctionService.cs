using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IJunctionService
{
    Task<IReadOnlyList<JunctionStatusDto>> GetAllAsync();

    Task<JunctionStatusDto?> GetStatusAsync(
        string junctionId);

    Task<IReadOnlyList<QueuedVehicleDto>>
        GetQueuedVehiclesAsync(string junctionId);

    Task SetModeAsync(
        string junctionId,
        JunctionModeRequest request);
}