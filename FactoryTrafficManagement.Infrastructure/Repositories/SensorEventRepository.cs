using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FactoryTrafficManagement.Infrastructure.Repositories;

public class SensorEventRepository : ISensorEventRepository
{
    private readonly AppDbContext _context;

    public SensorEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SensorEvent?> GetByIdAsync(string eventId)
    {
        return await _context.SensorEvents
            .FirstOrDefaultAsync(x => x.EventId == eventId);
    }

    public async Task AddAsync(SensorEvent sensorEvent)
    {
        await _context.SensorEvents.AddAsync(sensorEvent);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}