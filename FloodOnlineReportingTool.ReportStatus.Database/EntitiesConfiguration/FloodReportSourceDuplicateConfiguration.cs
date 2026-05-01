using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceDuplicateConfiguration : IEntityTypeConfiguration<FloodReportSourceDuplicate>
{
    public void Configure(EntityTypeBuilder<FloodReportSourceDuplicate> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasIndex(o => o.EligibilityCheckId);

        // Explicit many-to-many is required here because FloodReport has no inverse
        // navigation property back to FloodReportSourceDuplicate. Without this,
        // EF Core conventions would incorrectly create a FK on FloodReport instead
        // of a junction table.
        builder
            .HasMany(o => o.FloodReports)
            .WithMany()
            .UsingEntity(j => j.ToTable("FloodReportSourceDuplicateFloodReport"));

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
