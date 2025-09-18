using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodPropertyFloodReportConfiguration : IEntityTypeConfiguration<FloodPropertyFloodReport>
{
    public void Configure(EntityTypeBuilder<FloodPropertyFloodReport> builder)
    {
        builder.HasKey(o => new { o.FloodPropertyId, o.FloodReportId });

        builder
            .Property(o => o.FloodPropertyId)
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
