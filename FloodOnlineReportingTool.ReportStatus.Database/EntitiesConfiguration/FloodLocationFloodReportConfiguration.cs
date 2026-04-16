using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodLocationFloodReportConfiguration : IEntityTypeConfiguration<FloodLocationFloodReport>
{
    public void Configure(EntityTypeBuilder<FloodLocationFloodReport> builder)
    {
        builder.HasKey(o => new { o.FloodLocationId, o.FloodReportId, o.FloodReportSourceId });

        builder
            .Property(o => o.FloodLocationId)
            .ValueGeneratedNever();

        builder
            .Property(o => o.FloodReportId)
            .ValueGeneratedNever();

        builder
            .Property(o => o.FloodReportSourceId)
            .ValueGeneratedNever();

        builder.HasIndex(o => o.ConfirmedByUserId);

        builder
            .Property(o => o.ConfirmedByUserId)
            .HasMaxLength(200);

        builder
            .Property(o => o.ConfirmedUserName)
            .HasMaxLength(2000);

        builder
            .HasOne(o => o.FloodLocation)
            .WithMany(fl => fl.FloodReports)
            .HasForeignKey(o => o.FloodLocationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.FloodReport)
            .WithMany()
            .HasForeignKey(o => new { o.FloodReportId, o.FloodReportSourceId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}
