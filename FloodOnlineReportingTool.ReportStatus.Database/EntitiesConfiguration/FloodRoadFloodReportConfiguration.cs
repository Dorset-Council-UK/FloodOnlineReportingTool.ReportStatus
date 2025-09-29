using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodRoadFloodReportConfiguration : IEntityTypeConfiguration<FloodRoadFloodReport>
{
    public void Configure(EntityTypeBuilder<FloodRoadFloodReport> builder)
    {
        builder.HasKey(o => new { o.FloodRoadId, o.FloodReportId });

        builder
            .Property(o => o.FloodRoadId)
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
