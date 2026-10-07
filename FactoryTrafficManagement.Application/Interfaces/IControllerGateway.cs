using FactoryTrafficManagement.Domain.Enums;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IControllerGateway
{
    Task SendSignalCommandAsync(
        string commandId,
        string junctionId,
        Direction direction,
        SignalState state);
}