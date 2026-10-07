using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FactoryTrafficManagement.Infrastructure.Repositories;

public class JunctionRepository : IJunctionRepository
{
    private readonly AppDbContext _context;

    public JunctionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Junction>> GetAllAsync()
    {
        return await _context.Junctions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Junction?> GetByIdAsync(
        string junctionId)
    {
        return await _context.Junctions
            .FirstOrDefaultAsync(x => x.Id == junctionId);
    }

    public async Task SaveAsync(
        Junction junction)
    {
        await _context.SaveChangesAsync();
    }
}