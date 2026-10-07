using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.Rules;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Application.Services;

public class ControllerEventService : IControllerEventService
{
    private readonly IControllerCommandRepository _commandRepository;
    private readonly IJunctionRepository _junctionRepository;
    private readonly IAuditRepository _auditRepository;

    public ControllerEventService(
        IControllerCommandRepository commandRepository,
        IJunctionRepository junctionRepository,
        IAuditRepository auditRepository)
    {
        _commandRepository = commandRepository;
        _junctionRepository = junctionRepository;
        _auditRepository = auditRepository;
    }

    public async Task ProcessAcknowledgementAsync(
        ControllerEventRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CommandId))
        {
            throw new ArgumentException(
                "CommandId is required.");
        }

        var command =
            await _commandRepository
                .GetByIdAsync(request.CommandId);

        if (command == null)
        {
            throw new InvalidOperationException(
                $"Command '{request.CommandId}' was not found.");
        }

        var junction =
            await _junctionRepository
                .GetByIdAsync(command.JunctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{command.JunctionId}' was not found.");
        }

        // Controller rejected or failed the command.
        if (!request.Successful)
        {
            command.MarkFailed();

            var failureAudit = new Domain.Entities.AuditLog(
                junction.Id,
                "CONTROLLER_COMMAND_FAILED",
                $"Controller rejected command {command.CommandId}.");

            await _auditRepository.AddAsync(failureAudit);

            await _commandRepository.SaveChangesAsync();
            await _auditRepository.SaveChangesAsync();

            return;
        }

        /*
         * The controller has explicitly confirmed the
         * actual state. Only now do we update ActualSignals.
         */

        var actualSignals =
            junction.ActualSignals;

        var updatedSignals =
            request.ActualState switch
            {
                SignalState.Green =>
                    SetSignal(
                        actualSignals,
                        command.Direction,
                        SignalState.Green),

                SignalState.Yellow =>
                    SetSignal(
                        actualSignals,
                        command.Direction,
                        SignalState.Yellow),

                SignalState.Red =>
                    SetSignal(
                        actualSignals,
                        command.Direction,
                        SignalState.Red),

                _ =>
                    throw new ArgumentOutOfRangeException(
                        nameof(request.ActualState),
                        request.ActualState,
                        "Unknown signal state.")
            };

        TrafficSafetyRules.EnsureSafe(
            updatedSignals);

        command.MarkAcknowledged();

        junction.SetActualSignals(
            updatedSignals);

        await _commandRepository.SaveChangesAsync();
        await _junctionRepository.SaveAsync(junction);

        var audit = new Domain.Entities.AuditLog(
            junction.Id,
            "CONTROLLER_ACK",
            $"Controller acknowledged command {command.CommandId}. " +
            $"Actual {command.Direction} state is {request.ActualState}.");

        await _auditRepository.AddAsync(audit);
        await _auditRepository.SaveChangesAsync();
    }

    private static SignalSet SetSignal(
        SignalSet current,
        Direction direction,
        SignalState state)
    {
        return direction switch
        {
            Direction.North =>
                new SignalSet(
                    state,
                    current.South,
                    current.East,
                    current.West),

            Direction.South =>
                new SignalSet(
                    current.North,
                    state,
                    current.East,
                    current.West),

            Direction.East =>
                new SignalSet(
                    current.North,
                    current.South,
                    state,
                    current.West),

            Direction.West =>
                new SignalSet(
                    current.North,
                    current.South,
                    current.East,
                    state),

            _ =>
                throw new ArgumentOutOfRangeException(
                    nameof(direction),
                    direction,
                    "Unknown direction.")
        };
    }
}