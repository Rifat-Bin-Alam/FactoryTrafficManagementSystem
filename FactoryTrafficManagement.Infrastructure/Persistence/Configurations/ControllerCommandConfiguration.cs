using FactoryTrafficManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTrafficManagement.Infrastructure.Persistence.Configurations;

public class ControllerCommandConfiguration
    : IEntityTypeConfiguration<ControllerCommand>
{
    public void Configure(
        EntityTypeBuilder<ControllerCommand> builder)
    {
        builder.HasKey(x => x.CommandId);

        builder.Property(x => x.CommandId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.JunctionId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Direction)
            .IsRequired();

        builder.Property(x => x.RequestedState)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.CommandId)
            .IsUnique();

        builder.HasIndex(x => new
        {
            x.JunctionId,
            x.Status
        });
    }
}