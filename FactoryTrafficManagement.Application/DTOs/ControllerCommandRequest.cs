using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.DTOs;

public class ControllerCommandRequest
{
    public string JunctionId { get; set; } = string.Empty;

    public string CommandType { get; set; } = string.Empty;

    public Direction Direction { get; set; }
}