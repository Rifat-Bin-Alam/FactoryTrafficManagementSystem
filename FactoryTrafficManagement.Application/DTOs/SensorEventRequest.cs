using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.DTOs;

public class SensorEventRequest
{
    public string EventId { get; set; } = string.Empty;

    public string JunctionId { get; set; } = string.Empty;

    public Direction Direction { get; set; }

    public EventType EventType { get; set; }

    public string VehicleId { get; set; } = string.Empty;

    public VehicleType VehicleType { get; set; }

    public long SequenceNumber { get; set; }

    public DateTime Timestamp { get; set; }
}