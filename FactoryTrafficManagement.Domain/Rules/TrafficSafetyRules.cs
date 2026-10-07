using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Domain.Rules;

public static class TrafficSafetyRules
{
    public static bool IsSafe(SignalSet signals)
    {
        bool northSouthGreen =
            signals.North == SignalState.Green ||
            signals.South == SignalState.Green;

        bool eastWestGreen =
            signals.East == SignalState.Green ||
            signals.West == SignalState.Green;

        return !(northSouthGreen && eastWestGreen);
    }

    public static void EnsureSafe(SignalSet signals)
    {
        if (!IsSafe(signals))
        {
            throw new InvalidOperationException(
                "Unsafe signal state: conflicting directions cannot be GREEN at the same time.");
        }
    }
}