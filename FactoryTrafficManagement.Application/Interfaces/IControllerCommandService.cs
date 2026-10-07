using FactoryTrafficManagement.Application.DTOs;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IControllerCommandService
{
    Task<string> SendCommandAsync(
        ControllerCommandRequest request);
}