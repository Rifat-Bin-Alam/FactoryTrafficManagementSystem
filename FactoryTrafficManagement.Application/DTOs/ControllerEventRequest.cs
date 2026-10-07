using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.DTOs;

public class ControllerEventRequest
{
    public string CommandId { get; set; } = string.Empty;

    public string JunctionId { get; set; } = string.Empty;

    public bool Successful { get; set; }

    public string Status { get; set; } = string.Empty;

    public SignalState ActualState { get; set; }

    public DateTime Timestamp { get; set; }
}