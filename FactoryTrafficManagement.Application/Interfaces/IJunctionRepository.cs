using FactoryTrafficManagement.Domain.Entities;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IJunctionRepository
{
    Task<IReadOnlyList<Junction>> GetAllAsync();

    Task<Junction?> GetByIdAsync(
        string junctionId);

    Task SaveAsync(
        Junction junction);
}