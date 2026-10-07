using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.DTOs;

public class SignalSetDto
{
    public SignalState North { get; set; }

    public SignalState South { get; set; }

    public SignalState East { get; set; }

    public SignalState West { get; set; }
}