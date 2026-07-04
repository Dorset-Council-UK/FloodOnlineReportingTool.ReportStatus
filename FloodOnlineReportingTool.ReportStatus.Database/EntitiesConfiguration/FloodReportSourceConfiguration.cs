using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceConfiguration : IEntityTypeConfiguration<FloodReportSource>
{
    public void Configure(EntityTypeBuilder<FloodReportSource> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasAlternateKey(o => o.Reference);

        builder.HasIndex(o => o.EligibilityCheckId);
        builder.HasIndex(o => o.Reference).IsUnique();
        builder.HasIndex(o => new { o.FloodReportId, o.EligibilityCheckId });

        builder
            .Property(o => o.Geom)
            .HasColumnType("geometry (point, 27700)");

        builder
            .Property(o => o.Easting)
            .HasComputedColumnSql("ST_X(ST_Transform(\"Geom\", 27700))", stored: true);

        builder
            .Property(o => o.Northing)
            .HasComputedColumnSql("ST_Y(ST_Transform(\"Geom\", 27700))", stored: true);

        builder
            .Property(o => o.Latitude)
            .HasComputedColumnSql("ST_Y(ST_Transform(\"Geom\", 4326))", stored: true);

        builder
            .Property(o => o.Longitude)
            .HasComputedColumnSql("ST_X(ST_Transform(\"Geom\", 4326))", stored: true);
        
    }
}
