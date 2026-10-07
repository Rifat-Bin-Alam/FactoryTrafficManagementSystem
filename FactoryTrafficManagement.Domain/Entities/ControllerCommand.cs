using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Domain.Entities;

public class ControllerCommand
{
    public string CommandId { get; private set; }

    public string JunctionId { get; private set; }

    public Direction Direction { get; private set; }

    public SignalState RequestedState { get; private set; }

    public ControllerCommandStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? AcknowledgedAt { get; private set; }

    public ControllerCommand(
        string commandId,
        string junctionId,
        Direction direction,
        SignalState requestedState)
    {
        CommandId = commandId;
        JunctionId = junctionId;
        Direction = direction;
        RequestedState = requestedState;

        Status = ControllerCommandStatus.Pending;

        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAcknowledged()
    {
        Status = ControllerCommandStatus.Acknowledged;
        AcknowledgedAt = DateTime.UtcNow;
    }

    public void MarkFailed()
    {
        Status = ControllerCommandStatus.Failed;
    }

    public void MarkTimedOut()
    {
        Status = ControllerCommandStatus.TimedOut;
    }
}