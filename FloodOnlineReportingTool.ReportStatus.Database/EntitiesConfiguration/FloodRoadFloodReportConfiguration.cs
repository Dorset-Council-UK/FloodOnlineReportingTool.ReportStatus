using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodRoadFloodReportConfiguration : IEntityTypeConfiguration<FloodRoadFloodReport>
{
    public void Configure(EntityTypeBuilder<FloodRoadFloodReport> builder)
    {
        builder.HasKey(o => new { o.FloodRoadId, o.FloodReportId, o.FloodReportSourceId });

        builder.Property(o => o.FloodRoadId).ValueGeneratedNever();
        builder.Property(o => o.FloodReportId).ValueGeneratedNever();
        builder.Property(o => o.FloodReportSourceId).ValueGeneratedNever();

        builder.HasIndex(o => o.ConfirmedByUserId);
        builder.Property(o => o.ConfirmedByUserId).HasMaxLength(200);
        builder.Property(o => o.ConfirmedUserName).HasMaxLength(2000);

        builder
            .HasOne(o => o.FloodRoad)
            .WithMany(fr => fr.FloodReports)
            .HasForeignKey(o => o.FloodRoadId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.FloodReport)
            .WithMany()
            .HasForeignKey(o => new { o.FloodReportId, o.FloodReportSourceId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}
