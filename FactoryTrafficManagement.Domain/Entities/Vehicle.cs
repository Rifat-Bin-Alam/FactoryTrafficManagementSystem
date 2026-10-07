using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Domain.Entities;

public class Vehicle
{
    public string Id { get; private set; }

    public string JunctionId { get; private set; }

    public Direction Direction { get; private set; }

    public VehicleType Type { get; private set; }

    public DateTime ArrivedAt { get; private set; }

    public bool IsQueued { get; private set; }

    public long LastSequenceNumber { get; private set; }

    public Vehicle(
        string id,
        string junctionId,
        Direction direction,
        VehicleType type)
        : this(
            id,
            junctionId,
            direction,
            type,
            DateTime.UtcNow)
    {
    }

    public Vehicle(
        string id,
        string junctionId,
        Direction direction,
        VehicleType type,
        DateTime arrivedAt)
    {
        Id = id;
        JunctionId = junctionId;
        Direction = direction;
        Type = type;
        ArrivedAt = arrivedAt;
        IsQueued = true;
        LastSequenceNumber = 0;
    }

    public void MarkCleared()
    {
        IsQueued = false;
    }

    public bool AcceptSequence(long sequenceNumber)
    {
        if (sequenceNumber <= LastSequenceNumber)
        {
            return false;
        }

        LastSequenceNumber = sequenceNumber;

        return true;
    }
}