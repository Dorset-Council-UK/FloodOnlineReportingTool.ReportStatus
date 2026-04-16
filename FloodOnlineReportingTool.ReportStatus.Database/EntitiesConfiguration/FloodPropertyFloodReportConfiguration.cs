using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodPropertyFloodReportConfiguration : IEntityTypeConfiguration<FloodPropertyFloodReport>
{
    public void Configure(EntityTypeBuilder<FloodPropertyFloodReport> builder)
    {
        builder.HasKey(o => new { o.FloodPropertyId, o.FloodReportId, o.FloodReportSourceId });

        builder.Property(o => o.FloodPropertyId).ValueGeneratedNever();
        builder.Property(o => o.FloodReportId).ValueGeneratedNever();
        builder.Property(o => o.FloodReportSourceId).ValueGeneratedNever();

        builder.HasIndex(o => o.ConfirmedByUserId);
        builder.Property(o => o.ConfirmedByUserId).HasMaxLength(200);
        builder.Property(o => o.ConfirmedUserName).HasMaxLength(2000);

        builder
            .HasOne(o => o.FloodProperty)
            .WithMany(fp => fp.FloodReports)
            .HasForeignKey(o => o.FloodPropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(o => o.FloodReport)
            .WithMany()
            .HasForeignKey(o => new { o.FloodReportId, o.FloodReportSourceId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}
