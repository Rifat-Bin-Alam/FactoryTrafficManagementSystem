using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.StateMachine;

namespace FactoryTrafficManagement.Tests.Domain;

public class TrafficStateMachineTests
{
    private readonly TrafficStateMachine _stateMachine = new();

    [Fact]
    public void NorthSouthToEastWest_ShouldFollowSafeTransition()
    {
        var transition = _stateMachine.GetSafeTransition(
            TrafficPhase.NorthSouth,
            TrafficPhase.EastWest);

        Assert.Equal(3, transition.Count);

        Assert.Equal(
            SignalState.Yellow,
            transition[0].North);

        Assert.Equal(
            SignalState.Yellow,
            transition[0].South);

        Assert.Equal(
            SignalState.Red,
            transition[1].North);

        Assert.Equal(
            SignalState.Red,
            transition[1].East);

        Assert.Equal(
            SignalState.Green,
            transition[2].East);

        Assert.Equal(
            SignalState.Green,
            transition[2].West);
    }

    [Fact]
    public void EastWestToNorthSouth_ShouldFollowSafeTransition()
    {
        var transition = _stateMachine.GetSafeTransition(
            TrafficPhase.EastWest,
            TrafficPhase.NorthSouth);

        Assert.Equal(3, transition.Count);

        Assert.Equal(
            SignalState.Yellow,
            transition[0].East);

        Assert.Equal(
            SignalState.Yellow,
            transition[0].West);

        Assert.Equal(
            SignalState.Red,
            transition[1].North);

        Assert.Equal(
            SignalState.Red,
            transition[1].East);

        Assert.Equal(
            SignalState.Green,
            transition[2].North);

        Assert.Equal(
            SignalState.Green,
            transition[2].South);
    }

    [Fact]
    public void NorthSouthToAllRed_ShouldPassThroughYellow()
    {
        var transition = _stateMachine.GetSafeTransition(
            TrafficPhase.NorthSouth,
            TrafficPhase.AllRed);

        Assert.Equal(2, transition.Count);

        Assert.Equal(
            SignalState.Yellow,
            transition[0].North);

        Assert.Equal(
            SignalState.Yellow,
            transition[0].South);

        Assert.Equal(
            SignalState.Red,
            transition[1].North);

        Assert.Equal(
            SignalState.Red,
            transition[1].South);

        Assert.Equal(
            SignalState.Red,
            transition[1].East);

        Assert.Equal(
            SignalState.Red,
            transition[1].West);
    }

    [Fact]
    public void AllRedToEastWest_ShouldGoDirectlyToEastWestGreen()
    {
        var transition = _stateMachine.GetSafeTransition(
            TrafficPhase.AllRed,
            TrafficPhase.EastWest);

        Assert.Single(transition);

        Assert.Equal(
            SignalState.Green,
            transition[0].East);

        Assert.Equal(
            SignalState.Green,
            transition[0].West);

        Assert.Equal(
            SignalState.Red,
            transition[0].North);

        Assert.Equal(
            SignalState.Red,
            transition[0].South);
    }

    [Fact]
    public void SamePhase_ShouldRequireNoTransition()
    {
        var transition = _stateMachine.GetSafeTransition(
            TrafficPhase.NorthSouth,
            TrafficPhase.NorthSouth);

        Assert.Empty(transition);
    }
}