using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.DTOs;

public class TrafficTransitionDto
{
    public TrafficPhase FromPhase { get; set; }

    public TrafficPhase ToPhase { get; set; }

    public List<SignalSetDto> Steps { get; set; } = new();
}