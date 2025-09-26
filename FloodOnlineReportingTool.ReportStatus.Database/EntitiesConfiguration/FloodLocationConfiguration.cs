using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodLocationConfiguration : IEntityTypeConfiguration<FloodLocation>
{
    public void Configure(EntityTypeBuilder<FloodLocation> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .Property(o => o.Id)
            .ValueGeneratedNever();

        builder
            .Property(o => o.Location)
            .HasColumnType("geometry (point)");
    }
}
