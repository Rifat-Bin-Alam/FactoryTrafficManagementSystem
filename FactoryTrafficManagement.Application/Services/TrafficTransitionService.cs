using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.Scheduling;
using FactoryTrafficManagement.Domain.StateMachine;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Application.Services;

public class TrafficTransitionService
    : ITrafficTransitionService
{
    private readonly IJunctionRepository _junctionRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IControllerGateway _controllerGateway;

    private readonly TrafficScheduler _scheduler;
    private readonly TrafficStateMachine _stateMachine;

    public TrafficTransitionService(
        IJunctionRepository junctionRepository,
        IVehicleRepository vehicleRepository,
        IControllerGateway controllerGateway)
    {
        _junctionRepository = junctionRepository;
        _vehicleRepository = vehicleRepository;
        _controllerGateway = controllerGateway;

        _scheduler = new TrafficScheduler();
        _stateMachine = new TrafficStateMachine();
    }

    public async Task ExecuteAsync(
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
            return;
        }

        Direction direction;

        if (junction.Mode == JunctionMode.Emergency)
        {
            var emergency =
                vehicles
                    .Where(v =>
                        v.Type == VehicleType.Emergency)
                    .OrderBy(v => v.ArrivedAt)
                    .FirstOrDefault();

            if (emergency == null)
            {
                return;
            }

            direction = emergency.Direction;
        }
        else if (junction.Mode == JunctionMode.Automatic)
        {
            direction =
                _scheduler.SelectNextDirection(
                    vehicles,
                    DateTime.UtcNow);
        }
        else
        {
            return;
        }

        var targetPhase =
            direction == Direction.North ||
            direction == Direction.South
                ? TrafficPhase.NorthSouth
                : TrafficPhase.EastWest;

        var steps =
            _stateMachine.GetSafeTransition(
                junction.CurrentPhase,
                targetPhase);

        foreach (var signals in steps)
        {
            await SendSignalSetAsync(
                junctionId,
                signals);
        }

        junction.SetDesiredSignals(
            _stateMachine.GetSignalsForPhase(
                targetPhase));

        junction.SetPhase(targetPhase);

        await _junctionRepository.SaveAsync(
            junction);
    }

    private async Task SendSignalSetAsync(
        string junctionId,
        SignalSet signals)
    {
        await SendIfNeededAsync(
            junctionId,
            Direction.North,
            signals.North);

        await SendIfNeededAsync(
            junctionId,
            Direction.South,
            signals.South);

        await SendIfNeededAsync(
            junctionId,
            Direction.East,
            signals.East);

        await SendIfNeededAsync(
            junctionId,
            Direction.West,
            signals.West);
    }

    private async Task SendIfNeededAsync(
        string junctionId,
        Direction direction,
        SignalState state)
    {
        var commandId =
            Guid.NewGuid().ToString();

        await _controllerGateway
            .SendSignalCommandAsync(
                commandId,
                junctionId,
                direction,
                state);
    }
}