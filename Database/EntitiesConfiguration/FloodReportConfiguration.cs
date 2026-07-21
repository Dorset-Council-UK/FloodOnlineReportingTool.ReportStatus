using FloodOnlineReportingTool.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.Database.EntitiesConfiguration;

internal class FloodReportConfiguration : IEntityTypeConfiguration<FloodReport>
{
    public void Configure(EntityTypeBuilder<FloodReport> builder)
    {
        builder.HasKey(o => o.Id);
        // Unique constraint ensures Reference is consistent across FORT modules
        builder.HasAlternateKey(o => o.Reference);

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