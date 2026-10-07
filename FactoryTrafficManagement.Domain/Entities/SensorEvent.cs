using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Domain.Entities;

public class SensorEvent
{
    public string EventId { get; private set; }

    public string JunctionId { get; private set; }

    public Direction Direction { get; private set; }

    public EventType EventType { get; private set; }

    public string VehicleId { get; private set; }

    public VehicleType VehicleType { get; private set; }

    public long SequenceNumber { get; private set; }

    public DateTime SensorTimestamp { get; private set; }

    public DateTime ReceivedAt { get; private set; }

    public bool Processed { get; private set; }

    public SensorEvent(
        string eventId,
        string junctionId,
        Direction direction,
        EventType eventType,
        string vehicleId,
        VehicleType vehicleType,
        long sequenceNumber,
        DateTime sensorTimestamp)
    {
        EventId = eventId;
        JunctionId = junctionId;
        Direction = direction;
        EventType = eventType;
        VehicleId = vehicleId;
        VehicleType = vehicleType;
        SequenceNumber = sequenceNumber;
        SensorTimestamp = sensorTimestamp;
        ReceivedAt = DateTime.UtcNow;
        Processed = false;
    }

    public void MarkProcessed()
    {
        Processed = true;
    }
}