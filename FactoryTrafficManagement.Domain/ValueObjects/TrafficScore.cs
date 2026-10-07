namespace FactoryTrafficManagement.Domain.ValueObjects;

public sealed class TrafficScore
{
    public double QueueScore { get; }

    public double PriorityScore { get; }

    public double WaitingScore { get; }

    public double StarvationScore { get; }

    public double TotalScore =>
        QueueScore +
        PriorityScore +
        WaitingScore +
        StarvationScore;

    public TrafficScore(
        double queueScore,
        double priorityScore,
        double waitingScore,
        double starvationScore)
    {
        QueueScore = queueScore;
        PriorityScore = priorityScore;
        WaitingScore = waitingScore;
        StarvationScore = starvationScore;
    }
}