using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Scheduling;

namespace FactoryTrafficManagement.Application.Services;

public class TrafficDecisionService : ITrafficDecisionService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly TrafficScheduler _scheduler;

    public TrafficDecisionService(
        IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
        _scheduler = new TrafficScheduler();
    }

    public async Task<string> GetNextDirectionAsync(
        string junctionId)
    {
        var vehicles =
            await _vehicleRepository
                .GetQueuedByJunctionIdAsync(junctionId);

        if (vehicles.Count == 0)
        {
            throw new InvalidOperationException(
                $"No queued vehicles exist at junction '{junctionId}'.");
        }

        var direction = _scheduler.SelectNextDirection(
            vehicles,
            DateTime.UtcNow);

        return direction.ToString();
    }
}