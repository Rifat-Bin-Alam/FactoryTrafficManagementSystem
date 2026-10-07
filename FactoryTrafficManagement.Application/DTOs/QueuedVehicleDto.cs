using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.DTOs;

public class QueuedVehicleDto
{
    public string VehicleId { get; set; } = string.Empty;

    public string JunctionId { get; set; } = string.Empty;

    public Direction Direction { get; set; }

    public VehicleType VehicleType { get; set; }

    public DateTime ArrivedAt { get; set; }

    public double WaitingSeconds { get; set; }
}