using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Infrastructure.Controllers;

public class SimulatedControllerGateway : IControllerGateway
{
    private readonly IControllerCommandRepository _commandRepository;
    private readonly IJunctionRepository _junctionRepository;

    public SimulatedControllerGateway(
        IControllerCommandRepository commandRepository,
        IJunctionRepository junctionRepository)
    {
        _commandRepository = commandRepository;
        _junctionRepository = junctionRepository;
    }

    public async Task SendSignalCommandAsync(
        string commandId,
        string junctionId,
        Direction direction,
        SignalState state)
    {
        Console.WriteLine(
            $"[SIMULATED CONTROLLER] " +
            $"Command={commandId}, " +
            $"Junction={junctionId}, " +
            $"Direction={direction}, " +
            $"State={state}");

        // Simulate successful controller execution.
        var junction =
            await _junctionRepository.GetByIdAsync(junctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{junctionId}' was not found.");
        }

        var command =
            await _commandRepository.GetByIdAsync(commandId);

        if (command == null)
        {
            throw new InvalidOperationException(
                $"Command '{commandId}' was not found.");
        }

        var currentSignals = junction.ActualSignals;

        var updatedSignals =
            direction switch
            {
                Direction.North =>
                    new Domain.ValueObjects.SignalSet(
                        state,
                        currentSignals.South,
                        currentSignals.East,
                        currentSignals.West),

                Direction.South =>
                    new Domain.ValueObjects.SignalSet(
                        currentSignals.North,
                        state,
                        currentSignals.East,
                        currentSignals.West),

                Direction.East =>
                    new Domain.ValueObjects.SignalSet(
                        currentSignals.North,
                        currentSignals.South,
                        state,
                        currentSignals.West),

                Direction.West =>
                    new Domain.ValueObjects.SignalSet(
                        currentSignals.North,
                        currentSignals.South,
                        currentSignals.East,
                        state),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(direction),
                    direction,
                    "Unknown direction.")
            };

        // Never allow the simulated controller
        // to create conflicting GREEN signals.
        FactoryTrafficManagement.Domain.Rules
            .TrafficSafetyRules
            .EnsureSafe(updatedSignals);

        // The physical controller has confirmed the state.
        junction.SetActualSignals(updatedSignals);

        command.MarkAcknowledged();

        await _junctionRepository.SaveAsync(junction);
        await _commandRepository.SaveChangesAsync();

        Console.WriteLine(
            $"[SIMULATED CONTROLLER ACK] " +
            $"Command={commandId} acknowledged.");
    }
}