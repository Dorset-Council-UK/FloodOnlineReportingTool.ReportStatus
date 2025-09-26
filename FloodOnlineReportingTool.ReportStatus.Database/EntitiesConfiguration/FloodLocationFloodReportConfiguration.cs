using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodLocationFloodReportConfiguration : IEntityTypeConfiguration<FloodLocationFloodReport>
{
    public void Configure(EntityTypeBuilder<FloodLocationFloodReport> builder)
    {
        builder.HasKey(o => new { o.FloodLocationId, o.FloodReportId });

        builder
            .Property(o => o.FloodLocationId)
            .ValueGeneratedNever();

        builder
            .Property(o => o.FloodReportId)
            .ValueGeneratedNever();

        builder.HasIndex(o => o.ConfirmedByUserId);

        builder
            .Property(o => o.ConfirmedByUserId)
            .HasMaxLength(200);

        builder
            .Property(o => o.ConfirmedUserName)
            .HasMaxLength(2000);
    }
}
