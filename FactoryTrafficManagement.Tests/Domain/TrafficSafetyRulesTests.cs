using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.Rules;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Tests.Domain;

public class TrafficSafetyRulesTests
{
    [Fact]
    public void NorthSouthGreen_ShouldBeSafe()
    {
        var signals = SignalSet.NorthSouthGreen();

        var result = TrafficSafetyRules.IsSafe(signals);

        Assert.True(result);
    }

    [Fact]
    public void EastWestGreen_ShouldBeSafe()
    {
        var signals = SignalSet.EastWestGreen();

        var result = TrafficSafetyRules.IsSafe(signals);

        Assert.True(result);
    }

    [Fact]
    public void AllRed_ShouldBeSafe()
    {
        var signals = SignalSet.AllRed();

        var result = TrafficSafetyRules.IsSafe(signals);

        Assert.True(result);
    }

    [Fact]
    public void ConflictingGreenSignals_ShouldBeUnsafe()
    {
        var signals = new SignalSet(
            SignalState.Green,
            SignalState.Red,
            SignalState.Green,
            SignalState.Red);

        var result = TrafficSafetyRules.IsSafe(signals);

        Assert.False(result);
    }

    [Fact]
    public void EnsureSafe_ShouldThrowForConflictingGreenSignals()
    {
        var signals = new SignalSet(
            SignalState.Green,
            SignalState.Red,
            SignalState.Green,
            SignalState.Red);

        Assert.Throws<InvalidOperationException>(
            () => TrafficSafetyRules.EnsureSafe(signals));
    }
}