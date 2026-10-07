using FactoryTrafficManagement.Domain.Enums;
using FactoryTrafficManagement.Domain.ValueObjects;

namespace FactoryTrafficManagement.Domain.Entities;

public class Junction
{
    public string Id { get; private set; }

    public string Name { get; private set; }

    public JunctionMode Mode { get; private set; }

    public TrafficPhase CurrentPhase { get; private set; }

    public SignalSet DesiredSignals { get; private set; }

    public SignalSet ActualSignals { get; private set; }

    public SignalState DesiredNorth { get; private set; }

    public SignalState DesiredSouth { get; private set; }

    public SignalState DesiredEast { get; private set; }

    public SignalState DesiredWest { get; private set; }

    public SignalState ActualNorth { get; private set; }

    public SignalState ActualSouth { get; private set; }

    public SignalState ActualEast { get; private set; }

    public SignalState ActualWest { get; private set; }

    public bool ControllerOnline { get; private set; }

    public DateTime PhaseStartedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }


    public Junction(
        string id,
        string name)
    {
        Id = id;
        Name = name;

        // Junction starts in Automatic mode.
        Mode = JunctionMode.Automatic;

        // Initial traffic phase.
        CurrentPhase = TrafficPhase.NorthSouth;

        // Desired state starts with North/South GREEN.
        DesiredSignals =
            SignalSet.NorthSouthGreen();

        // Physical controller starts with all signals RED.
        // The actual state will change only after
        // the simulated controller confirms commands.
        ActualSignals =
            SignalSet.AllRed();


        DesiredNorth = SignalState.Green;
        DesiredSouth = SignalState.Green;
        DesiredEast = SignalState.Red;
        DesiredWest = SignalState.Red;


        ActualNorth = SignalState.Red;
        ActualSouth = SignalState.Red;
        ActualEast = SignalState.Red;
        ActualWest = SignalState.Red;


        // Simulated controller starts online.
        ControllerOnline = true;


        PhaseStartedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }


    public void SetDesiredSignals(
        SignalSet signals)
    {
        DesiredSignals = signals;

        DesiredNorth = signals.North;
        DesiredSouth = signals.South;
        DesiredEast = signals.East;
        DesiredWest = signals.West;

        UpdatedAt = DateTime.UtcNow;
    }


    public void SetActualSignals(
        SignalSet signals)
    {
        ActualSignals = signals;

        ActualNorth = signals.North;
        ActualSouth = signals.South;
        ActualEast = signals.East;
        ActualWest = signals.West;

        UpdatedAt = DateTime.UtcNow;
    }


    public void SetPhase(
        TrafficPhase phase)
    {
        CurrentPhase = phase;

        PhaseStartedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }


    public void SetMode(
        JunctionMode mode)
    {
        Mode = mode;

        UpdatedAt = DateTime.UtcNow;
    }


    public void SetControllerOnline(
        bool online)
    {
        ControllerOnline = online;

        UpdatedAt = DateTime.UtcNow;
    }


    public void SetDegraded()
    {
        Mode = JunctionMode.Degraded;

        ControllerOnline = false;

        UpdatedAt = DateTime.UtcNow;
    }


    public void SetRecovered()
    {
        // Restoring the controller does NOT
        // automatically change the operating mode.
        ControllerOnline = true;

        UpdatedAt = DateTime.UtcNow;
    }
}