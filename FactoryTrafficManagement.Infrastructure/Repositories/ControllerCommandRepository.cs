using FactoryTrafficManagement.Application.Interfaces;
using FactoryTrafficManagement.Domain.Entities;
using FactoryTrafficManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FactoryTrafficManagement.Infrastructure.Repositories;

public class ControllerCommandRepository
    : IControllerCommandRepository
{
    private readonly AppDbContext _context;

    public ControllerCommandRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<ControllerCommand?> GetByIdAsync(
        string commandId)
    {
        return await _context.ControllerCommands
            .FirstOrDefaultAsync(
                x => x.CommandId == commandId);
    }

    public async Task AddAsync(
        ControllerCommand command)
    {
        await _context.ControllerCommands.AddAsync(command);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}