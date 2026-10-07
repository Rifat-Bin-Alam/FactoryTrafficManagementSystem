using FactoryTrafficManagement.Domain.Entities;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IAuditRepository
{
    Task AddAsync(AuditLog auditLog);

    Task SaveChangesAsync();

    Task<IReadOnlyList<AuditLog>> GetByJunctionIdAsync(
        string junctionId);
}