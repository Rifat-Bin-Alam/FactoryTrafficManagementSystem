namespace FactoryTrafficManagement.Domain.Entities;

public class AuditLog
{
    public long Id { get; private set; }

    public string JunctionId { get; private set; }

    public string EventType { get; private set; }

    public string? Direction { get; private set; }

    public string? PreviousState { get; private set; }

    public string? NewState { get; private set; }

    public string? CommandId { get; private set; }

    public string Message { get; private set; }

    public DateTime Timestamp { get; private set; }

    public AuditLog(
        string junctionId,
        string eventType,
        string message)
    {
        JunctionId = junctionId;
        EventType = eventType;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}