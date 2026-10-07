using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Domain.StateMachine;

public sealed class SignalTransition
{
    public SignalSet From { get; }
    public SignalSet To { get; }
    public TrafficPhase FromPhase { get; }
    public TrafficPhase ToPhase { get; }

    public SignalTransition(
        SignalSet from,
        SignalSet to,
        TrafficPhase fromPhase,
        TrafficPhase toPhase)
    {
        From = from;
        To = to;
        FromPhase = fromPhase;
        ToPhase = toPhase;
    }
}