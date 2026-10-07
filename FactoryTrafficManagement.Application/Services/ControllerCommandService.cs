using FactoryTrafficManagement.Application.DTOs;
using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.Rules;
using FactoryTrafficManagement.Domain.StateMachine;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Application.Services;

public class ControllerCommandService : IControllerCommandService
{
    private readonly IControllerCommandRepository _commandRepository;
    private readonly IControllerGateway _controllerGateway;
    private readonly IJunctionRepository _junctionRepository;
    private readonly IAuditRepository _auditRepository;

    private readonly TrafficStateMachine _stateMachine;

    public ControllerCommandService(
        IControllerCommandRepository commandRepository,
        IControllerGateway controllerGateway,
        IJunctionRepository junctionRepository,
        IAuditRepository auditRepository)
    {
        _commandRepository = commandRepository;
        _controllerGateway = controllerGateway;
        _junctionRepository = junctionRepository;
        _auditRepository = auditRepository;

        _stateMachine = new TrafficStateMachine();
    }

    public async Task<string> SendCommandAsync(
        ControllerCommandRequest request)
    {
        ValidateRequest(request);

        var junction =
            await _junctionRepository
                .GetByIdAsync(request.JunctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{request.JunctionId}' was not found.");
        }

        if (request.CommandType == "RETURN_TO_AUTOMATIC")
        {
            junction.SetMode(JunctionMode.Automatic);

            await _junctionRepository.SaveAsync(junction);

            var audit = new AuditLog(
                junction.Id,
                "RETURN_TO_AUTOMATIC",
                "Manual control ended. Junction returned to Automatic mode.");

            await _auditRepository.AddAsync(audit);
            await _auditRepository.SaveChangesAsync();

            return "RETURNED_TO_AUTOMATIC";
        }

        if (junction.Mode != JunctionMode.Manual)
        {
            throw new InvalidOperationException(
                "Manual green requests are only allowed in Manual mode.");
        }

        var targetPhase =
            request.Direction == Direction.North ||
            request.Direction == Direction.South
                ? TrafficPhase.NorthSouth
                : TrafficPhase.EastWest;

        var transition =
            _stateMachine.GetSafeTransition(
                junction.CurrentPhase,
                targetPhase);

        if (transition.Count == 0)
        {
            return "NO_TRANSITION_REQUIRED";
        }

        string lastCommandId = string.Empty;

        foreach (var signalSet in transition)
        {
            TrafficSafetyRules.EnsureSafe(signalSet);

            lastCommandId =
                await SendSignalSetAsync(
                    request.JunctionId,
                    signalSet);
        }

        junction.SetDesiredSignals(
            _stateMachine.GetSignalsForPhase(
                targetPhase));

        junction.SetPhase(targetPhase);

        await _junctionRepository.SaveAsync(junction);

        var transitionAudit = new AuditLog(
            junction.Id,
            "MANUAL_TRANSITION",
            $"Manual request for {request.Direction} executed using a safe transition to {targetPhase}.");

        await _auditRepository.AddAsync(transitionAudit);
        await _auditRepository.SaveChangesAsync();

        return lastCommandId;
    }

    private async Task<string> SendSignalSetAsync(
        string junctionId,
        SignalSet signals)
    {
        string lastCommandId = string.Empty;

        lastCommandId =
            await SendSignalAsync(
                junctionId,
                Direction.North,
                signals.North);

        lastCommandId =
            await SendSignalAsync(
                junctionId,
                Direction.South,
                signals.South);

        lastCommandId =
            await SendSignalAsync(
                junctionId,
                Direction.East,
                signals.East);

        lastCommandId =
            await SendSignalAsync(
                junctionId,
                Direction.West,
                signals.West);

        return lastCommandId;
    }

    private async Task<string> SendSignalAsync(
        string junctionId,
        Direction direction,
        SignalState state)
    {
        var commandId = Guid.NewGuid().ToString();

        var command = new ControllerCommand(
            commandId,
            junctionId,
            direction,
            state);

        await _commandRepository.AddAsync(command);
        await _commandRepository.SaveChangesAsync();

        await _controllerGateway.SendSignalCommandAsync(
            commandId,
            junctionId,
            direction,
            state);

        return commandId;
    }

    private static void ValidateRequest(
        ControllerCommandRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.JunctionId))
        {
            throw new ArgumentException(
                "JunctionId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.CommandType))
        {
            throw new ArgumentException(
                "CommandType is required.");
        }

        if (request.CommandType != "MANUAL_GREEN_REQUEST" &&
            request.CommandType != "RETURN_TO_AUTOMATIC")
        {
            throw new ArgumentException(
                "Unsupported command type.");
        }
    }
}