using FactoryTrafficManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTrafficManagement.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration
    : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(
        EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.JunctionId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Direction)
            .HasMaxLength(20);

        builder.Property(x => x.PreviousState)
            .HasMaxLength(100);

        builder.Property(x => x.NewState)
            .HasMaxLength(100);

        builder.Property(x => x.CommandId)
            .HasMaxLength(100);

        builder.Property(x => x.Message)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.Timestamp)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.JunctionId,
            x.Timestamp
        });
    }
}