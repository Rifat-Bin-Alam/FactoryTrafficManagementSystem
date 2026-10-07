using FactoryTrafficManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FactoryTrafficManagement.Infrastructure.Persistence.Configurations;

public class JunctionConfiguration : IEntityTypeConfiguration<Junction>
{
    public void Configure(EntityTypeBuilder<Junction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Mode)
            .IsRequired();

        builder.Property(x => x.CurrentPhase)
            .IsRequired();

        builder.Property(x => x.ControllerOnline)
            .IsRequired();

        builder.Property(x => x.PhaseStartedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.Ignore(x => x.DesiredSignals);
        builder.Ignore(x => x.ActualSignals);
    }
}