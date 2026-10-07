using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.Scheduling;
using FactoryTrafficManagement.Domain.StateMachine;

namespace FactoryTrafficManagement.Application.Services;

public class AutomaticTrafficService : IAutomaticTrafficService
{
    private readonly IJunctionRepository _junctionRepository;
    private readonly IVehicleRepository _vehicleRepository;

    private readonly TrafficScheduler _scheduler;
    private readonly TrafficStateMachine _stateMachine;

    public AutomaticTrafficService(
        IJunctionRepository junctionRepository,
        IVehicleRepository vehicleRepository)
    {
        _junctionRepository = junctionRepository;
        _vehicleRepository = vehicleRepository;

        _scheduler = new TrafficScheduler();
        _stateMachine = new TrafficStateMachine();
    }

    public async Task<TrafficTransitionDto> EvaluateAsync(
        string junctionId)
    {
        var junction =
            await _junctionRepository.GetByIdAsync(
                junctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{junctionId}' was not found.");
        }

        var vehicles =
            await _vehicleRepository
                .GetQueuedByJunctionIdAsync(junctionId);

        if (vehicles.Count == 0)
        {
            throw new InvalidOperationException(
                $"No queued vehicles exist at junction '{junctionId}'.");
        }

        Direction selectedDirection;

        // Emergency mode always gives priority
        // to the queued emergency vehicle.
        if (junction.Mode == JunctionMode.Emergency)
        {
            var emergencyVehicle =
                vehicles
                    .Where(v =>
                        v.Type == VehicleType.Emergency)
                    .OrderBy(v => v.ArrivedAt)
                    .FirstOrDefault();

            if (emergencyVehicle == null)
            {
                throw new InvalidOperationException(
                    "Emergency mode is active but no emergency vehicle is queued.");
            }

            selectedDirection =
                emergencyVehicle.Direction;
        }
        else if (junction.Mode == JunctionMode.Automatic)
        {
            selectedDirection =
                _scheduler.SelectNextDirection(
                    vehicles,
                    DateTime.UtcNow);
        }
        else
        {
            throw new InvalidOperationException(
                $"Junction '{junctionId}' cannot be automatically evaluated while in {junction.Mode} mode.");
        }

        var targetPhase =
            GetPhaseForDirection(selectedDirection);

        var transition =
            _stateMachine.GetSafeTransition(
                junction.CurrentPhase,
                targetPhase);

        return new TrafficTransitionDto
        {
            FromPhase = junction.CurrentPhase,
            ToPhase = targetPhase,

            Steps = transition
                .Select(signals => new SignalSetDto
                {
                    North = signals.North,
                    South = signals.South,
                    East = signals.East,
                    West = signals.West
                })
                .ToList()
        };
    }

    private static TrafficPhase GetPhaseForDirection(
        Direction direction)
    {
        return direction switch
        {
            Direction.North =>
                TrafficPhase.NorthSouth,

            Direction.South =>
                TrafficPhase.NorthSouth,

            Direction.East =>
                TrafficPhase.EastWest,

            Direction.West =>
                TrafficPhase.EastWest,

            _ => throw new ArgumentOutOfRangeException(
                nameof(direction),
                direction,
                "Unknown direction.")
        };
    }
}