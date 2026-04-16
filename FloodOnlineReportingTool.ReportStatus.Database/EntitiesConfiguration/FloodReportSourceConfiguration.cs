using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceConfiguration : IEntityTypeConfiguration<FloodReportSource>
{
    public void Configure(EntityTypeBuilder<FloodReportSource> builder)
    {
        builder.HasKey(o => new { o.FloodReportId, o.Id });

        builder
            .Property(o => o.FloodReportId)
            .ValueGeneratedNever();

        builder
            .Property(o => o.Id)
            .ValueGeneratedNever();

        // Unique per FloodReportSource: same CauseId can exist across different sources
        builder.HasMany(o => o.FloodCauses)
            .WithOne()
            .HasForeignKey(fc => new { fc.FloodReportId, fc.FloodReportSourceId })
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Organisations)
            .WithOne()
            .HasForeignKey(org => new { org.FloodReportId, org.OrganisationId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}
