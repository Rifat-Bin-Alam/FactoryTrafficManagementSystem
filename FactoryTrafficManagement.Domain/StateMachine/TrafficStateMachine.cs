using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.Rules;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Domain.StateMachine;

public sealed class TrafficStateMachine
{
    public SignalSet GetSignalsForPhase(TrafficPhase phase)
    {
        return phase switch
        {
            TrafficPhase.NorthSouth =>
                SignalSet.NorthSouthGreen(),

            TrafficPhase.EastWest =>
                SignalSet.EastWestGreen(),

            TrafficPhase.AllRed =>
                SignalSet.AllRed(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(phase),
                phase,
                "Unknown traffic phase.")
        };
    }

    public IReadOnlyList<SignalSet> GetSafeTransition(
    TrafficPhase currentPhase,
    TrafficPhase targetPhase)
    {
        if (currentPhase == targetPhase)
        {
            return Array.Empty<SignalSet>();
        }

        var transition = new List<SignalSet>();

        // Every phase change must first go through YELLOW,
        // then ALL_RED, before the new phase becomes GREEN.
        if (currentPhase != TrafficPhase.AllRed)
        {
            transition.Add(GetYellowForPhase(currentPhase));
            transition.Add(SignalSet.AllRed());
        }

        if (targetPhase != TrafficPhase.AllRed)
        {
            transition.Add(GetSignalsForPhase(targetPhase));
        }

        foreach (var signals in transition)
        {
            TrafficSafetyRules.EnsureSafe(signals);
        }

        return transition;
    }

    private static SignalSet GetYellowForPhase(TrafficPhase phase)
    {
        return phase switch
        {
            TrafficPhase.NorthSouth =>
                new SignalSet(
                    SignalState.Yellow,
                    SignalState.Yellow,
                    SignalState.Red,
                    SignalState.Red),

            TrafficPhase.EastWest =>
                new SignalSet(
                    SignalState.Red,
                    SignalState.Red,
                    SignalState.Yellow,
                    SignalState.Yellow),

            TrafficPhase.AllRed =>
                SignalSet.AllRed(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(phase),
                phase,
                "Unknown traffic phase.")
        };
    }
}