using FactoryTrafficManagement.Application.DTOs;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IControllerEventService
{
    Task ProcessAcknowledgementAsync(
        ControllerEventRequest request);
}