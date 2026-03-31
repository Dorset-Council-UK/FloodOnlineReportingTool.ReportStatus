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
            .HasColumnType("geometry (point, 27700)");

        builder
            .Property(o => o.Latitude)
            .HasComputedColumnSql("ST_Y(ST_Transform(\"Geom\", 4326))", stored: true);

        builder
            .Property(o => o.Longitude)
            .HasComputedColumnSql("ST_X(ST_Transform(\"Geom\", 4326))", stored: true);
    }
}
