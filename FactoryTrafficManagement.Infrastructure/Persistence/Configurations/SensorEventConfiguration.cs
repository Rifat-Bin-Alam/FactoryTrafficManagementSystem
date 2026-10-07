using FactoryTrafficManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTrafficManagement.Infrastructure.Persistence.Configurations;

public class SensorEventConfiguration
    : IEntityTypeConfiguration<SensorEvent>
{
    public void Configure(
        EntityTypeBuilder<SensorEvent> builder)
    {
        builder.HasKey(x => x.EventId);

        builder.Property(x => x.EventId)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.EventId)
            .IsUnique();

        builder.Property(x => x.JunctionId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.VehicleId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Direction)
            .IsRequired();

        builder.Property(x => x.EventType)
            .IsRequired();

        builder.Property(x => x.VehicleType)
            .IsRequired();

        builder.Property(x => x.SequenceNumber)
            .IsRequired();

        builder.Property(x => x.SensorTimestamp)
            .IsRequired();

        builder.Property(x => x.ReceivedAt)
            .IsRequired();

        builder.Property(x => x.Processed)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.JunctionId,
            x.SequenceNumber
        });
    }
}