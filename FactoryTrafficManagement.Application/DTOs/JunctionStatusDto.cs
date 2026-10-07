using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.DTOs;

public class JunctionStatusDto
{
    public string JunctionId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public JunctionMode Mode { get; set; }

    public TrafficPhase CurrentPhase { get; set; }

    public bool ControllerOnline { get; set; }

    public DateTime PhaseStartedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public SignalSetDto DesiredSignals { get; set; }
        = new();

    public SignalSetDto ActualSignals { get; set; }
        = new();
}