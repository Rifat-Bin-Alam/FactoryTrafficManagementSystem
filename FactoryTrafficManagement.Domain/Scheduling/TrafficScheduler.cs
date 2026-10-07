using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Domain.Scheduling;

public sealed class TrafficScheduler
{
    private const double QueueWeight = 10.0;
    private const double PriorityWeight = 20.0;
    private const double WaitingWeight = 0.5;
    private const double StarvationThresholdSeconds = 60.0;
    private const double StarvationBonus = 100.0;

    public Direction SelectNextDirection(
        IReadOnlyCollection<Vehicle> vehicles,
        DateTime now)
    {
        var queuedVehicles = vehicles
            .Where(v => v.IsQueued)
            .ToList();

        if (queuedVehicles.Count == 0)
        {
            throw new InvalidOperationException(
                "Cannot select a direction when there are no queued vehicles.");
        }

        var scores = queuedVehicles
            .GroupBy(v => v.Direction)
            .Select(group => new
            {
                Direction = group.Key,
                Score = CalculateScore(group, now)
            })
            .OrderByDescending(x => x.Score.TotalScore)
            .ThenBy(x => x.Direction)
            .ToList();

        return scores.First().Direction;
    }

    public TrafficScore CalculateScore(
        IEnumerable<Vehicle> vehicles,
        DateTime now)
    {
        var vehicleList = vehicles
            .Where(v => v.IsQueued)
            .ToList();

        if (vehicleList.Count == 0)
        {
            return new TrafficScore(0, 0, 0, 0);
        }

        var queueScore =
            vehicleList.Count * QueueWeight;

        var highestPriority =
            vehicleList.Max(v => GetPriority(v.Type));

        var priorityScore =
            highestPriority * PriorityWeight;

        var averageWaitingSeconds =
            vehicleList
                .Select(v =>
                    Math.Max(
                        0,
                        (now - v.ArrivedAt).TotalSeconds))
                .Average();

        var waitingScore =
            averageWaitingSeconds * WaitingWeight;

        var starvationScore =
            averageWaitingSeconds >= StarvationThresholdSeconds
                ? StarvationBonus
                : 0;

        return new TrafficScore(
            queueScore,
            priorityScore,
            waitingScore,
            starvationScore);
    }

    private static int GetPriority(VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.Emergency => 4,
            VehicleType.Truck => 3,
            VehicleType.Forklift => 2,
            VehicleType.EmployeeVehicle => 1,

            _ => 0
        };
    }
}