using FactoryTrafficManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTrafficManagement.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration
    : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(
        EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.JunctionId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Direction)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.ArrivedAt)
            .IsRequired();

        builder.Property(x => x.IsQueued)
            .IsRequired();

        builder.Property(x => x.LastSequenceNumber)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.JunctionId,
            x.IsQueued
        });
    }
}