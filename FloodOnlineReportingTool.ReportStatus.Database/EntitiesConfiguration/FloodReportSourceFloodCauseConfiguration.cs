using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceFloodCauseConfiguration : IEntityTypeConfiguration<FloodReportSourceFloodCause>
{
    public void Configure(EntityTypeBuilder<FloodReportSourceFloodCause> builder)
    {
        builder.HasKey(o => o.Id);

        // Only enforces uniqueness for source-owned cause rows
        builder.HasIndex(o => new { o.FloodReportSourceId, o.CauseId })
            .IsUnique()
            .HasFilter("\"FloodReportSourceId\" IS NOT NULL");

        // Only enforces uniqueness for duplicate-owned cause rows
        builder.HasIndex(o => new { o.FloodReportSourceDuplicateId, o.CauseId })
            .IsUnique()
            .HasFilter("\"FloodReportSourceDuplicateId\" IS NOT NULL");

        builder.Property(fc => fc.Name).IsRequired().HasMaxLength(200);

        builder.HasOne(x => x.FloodReportSource)
            .WithMany(x => x.FloodCauses)
            .HasForeignKey(x => x.FloodReportSourceId)
            .IsRequired(false);

        builder.HasOne(x => x.FloodReportSourceDuplicate)
            .WithMany(x => x.FloodCauses)
            .HasForeignKey(x => x.FloodReportSourceDuplicateId)
            .IsRequired(false);
    }
}