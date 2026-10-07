using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FactoryTrafficManagement.Infrastructure.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly AppDbContext _context;

    public AuditRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog auditLog)
    {
        await _context.AuditLogs.AddAsync(auditLog);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<AuditLog>>
        GetByJunctionIdAsync(string junctionId)
    {
        return await _context.AuditLogs
            .AsNoTracking()
            .Where(x => x.JunctionId == junctionId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
}