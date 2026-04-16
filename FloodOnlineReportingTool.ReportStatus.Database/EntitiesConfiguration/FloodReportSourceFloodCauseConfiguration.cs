using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceFloodCauseConfiguration : IEntityTypeConfiguration<FloodReportSourceFloodCause>
{
    public void Configure(EntityTypeBuilder<FloodReportSourceFloodCause> builder)
    {
        builder.HasKey(fc => new { fc.FloodReportId, fc.FloodReportSourceId, fc.CauseId });

        builder.Property(fc => fc.Name).IsRequired().HasMaxLength(200);
    }
}