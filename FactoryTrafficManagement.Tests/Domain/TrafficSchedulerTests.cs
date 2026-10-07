using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.Scheduling;

namespace FactoryTrafficManagement.Tests.Domain;

public class TrafficSchedulerTests
{
    private readonly TrafficScheduler _scheduler = new();

    [Fact]
    public void EmergencyVehicle_ShouldBeatNormalTraffic()
    {
        var now = DateTime.UtcNow;

        var vehicles = new List<Vehicle>
        {
            new(
                "V-001",
                "A",
                Direction.North,
                VehicleType.Truck),

            new(
                "V-002",
                "A",
                Direction.East,
                VehicleType.Emergency)
        };

        var selectedDirection =
            _scheduler.SelectNextDirection(vehicles, now);

        Assert.Equal(Direction.East, selectedDirection);
    }

    [Fact]
    public void Truck_ShouldBeatForklift_WhenQueueSizesAreEqual()
    {
        var now = DateTime.UtcNow;

        var vehicles = new List<Vehicle>
        {
            new(
                "V-001",
                "A",
                Direction.North,
                VehicleType.Forklift),

            new(
                "V-002",
                "A",
                Direction.East,
                VehicleType.Truck)
        };

        var selectedDirection =
            _scheduler.SelectNextDirection(vehicles, now);

        Assert.Equal(Direction.East, selectedDirection);
    }

    [Fact]
    public void LargerQueue_ShouldReceiveHigherScore()
    {
        var now = DateTime.UtcNow;

        var vehicles = new List<Vehicle>
        {
            new(
                "V-001",
                "A",
                Direction.North,
                VehicleType.EmployeeVehicle),

            new(
                "V-002",
                "A",
                Direction.North,
                VehicleType.EmployeeVehicle),

            new(
                "V-003",
                "A",
                Direction.North,
                VehicleType.EmployeeVehicle),

            new(
                "V-004",
                "A",
                Direction.East,
                VehicleType.EmployeeVehicle)
        };

        var selectedDirection =
            _scheduler.SelectNextDirection(vehicles, now);

        Assert.Equal(Direction.North, selectedDirection);
    }

    [Fact]
    public void LongWaitingVehicle_ShouldReceiveStarvationBonus()
    {
        var now = DateTime.UtcNow;

        var vehicle = new Vehicle(
            "V-001",
            "A",
            Direction.West,
            VehicleType.EmployeeVehicle);

        // Simulate more than 60 seconds of waiting.
        typeof(Vehicle)
            .GetProperty(nameof(Vehicle.ArrivedAt))!
            .SetValue(
                vehicle,
                now.AddSeconds(-70));

        var score = _scheduler.CalculateScore(
            new[] { vehicle },
            now);

        Assert.True(score.StarvationScore > 0);
    }

    [Fact]
    public void ClearedVehicles_ShouldNotAffectScheduling()
    {
        var now = DateTime.UtcNow;

        var clearedVehicle = new Vehicle(
            "V-001",
            "A",
            Direction.North,
            VehicleType.Emergency);

        clearedVehicle.MarkCleared();

        var activeVehicle = new Vehicle(
            "V-002",
            "A",
            Direction.East,
            VehicleType.EmployeeVehicle);

        var vehicles = new List<Vehicle>
        {
            clearedVehicle,
            activeVehicle
        };

        var selectedDirection =
            _scheduler.SelectNextDirection(vehicles, now);

        Assert.Equal(Direction.East, selectedDirection);
    }

    [Fact]
    public void NoQueuedVehicles_ShouldThrowException()
    {
        var now = DateTime.UtcNow;

        var vehicles = new List<Vehicle>();

        Assert.Throws<InvalidOperationException>(
            () => _scheduler.SelectNextDirection(vehicles, now));
    }
}