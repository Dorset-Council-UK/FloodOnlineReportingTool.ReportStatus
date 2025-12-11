using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodRoadConfiguration : IEntityTypeConfiguration<FloodRoad>
{
    public void Configure(EntityTypeBuilder<FloodRoad> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .Property(o => o.Id)
            .ValueGeneratedNever();

        builder
            .Property(o => o.Geom)
            .HasColumnType("geometry (point)");
    }
}
