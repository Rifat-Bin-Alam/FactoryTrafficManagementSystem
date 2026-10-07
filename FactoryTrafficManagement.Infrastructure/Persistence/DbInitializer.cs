using FactoryTrafficManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FactoryTrafficManagement.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        

        if (await context.Junctions.AnyAsync())
        {
            return;
        }

        var junction = new Junction(
            "A",
            "Junction A");

        context.Junctions.Add(junction);

        await context.SaveChangesAsync();
    }
}