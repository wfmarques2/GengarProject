using DijitaruVatigoGengar.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DijitaruVatigoGengar.Data.Configs;

public class PendingHourConfig : IEntityTypeConfiguration<PendingHour>
{
    public void Configure(EntityTypeBuilder<PendingHour> builder)
    {
        builder
            .HasKey(pendingHour => pendingHour.Id);

        builder.Property(pendingHour => pendingHour.HourAmount)
            .IsRequired();

        builder.Property(pendingHour => pendingHour.IsApproved)
            .HasDefaultValue(false)
            .IsRequired();
    }
}