using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceConfiguration : IEntityTypeConfiguration<FloodReportSource>
{
    public void Configure(EntityTypeBuilder<FloodReportSource> builder)
    {
        builder.HasKey(o => new { o.FloodReportId, o.SourceId });

        builder
            .Property(o => o.FloodReportId)
            .ValueGeneratedNever();

        builder
            .Property(o => o.SourceId)
            .ValueGeneratedNever();

        builder
            .Property(o => o.Name)
            .HasMaxLength(200);
    }
}
