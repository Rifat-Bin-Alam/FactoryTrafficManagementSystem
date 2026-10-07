using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Domain.ValueObjects;

public sealed class SignalSet
{
    public SignalState North { get; }
    public SignalState South { get; }
    public SignalState East { get; }
    public SignalState West { get; }

    public SignalSet(
        SignalState north,
        SignalState south,
        SignalState east,
        SignalState west)
    {
        North = north;
        South = south;
        East = east;
        West = west;
    }

    public static SignalSet AllRed()
    {
        return new SignalSet(
            SignalState.Red,
            SignalState.Red,
            SignalState.Red,
            SignalState.Red);
    }

    public static SignalSet NorthSouthGreen()
    {
        return new SignalSet(
            SignalState.Green,
            SignalState.Green,
            SignalState.Red,
            SignalState.Red);
    }

    public static SignalSet EastWestGreen()
    {
        return new SignalSet(
            SignalState.Red,
            SignalState.Red,
            SignalState.Green,
            SignalState.Green);
    }
}