using FactoryTrafficManagement.Domain.Entities;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IControllerCommandRepository
{
    Task<ControllerCommand?> GetByIdAsync(
        string commandId);

    Task AddAsync(
        ControllerCommand command);

    Task SaveChangesAsync();
}