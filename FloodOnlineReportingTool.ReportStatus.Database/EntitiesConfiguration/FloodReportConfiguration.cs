using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportConfiguration : IEntityTypeConfiguration<FloodReport>
{
    public void Configure(EntityTypeBuilder<FloodReport> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .Property(o => o.Id)
            .ValueGeneratedNever();

        builder
            .Navigation(o =>o.Sources)
            .AutoInclude();
    }
}
