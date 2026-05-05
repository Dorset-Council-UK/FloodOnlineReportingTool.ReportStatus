using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportDuplicateReferenceConfiguration : IEntityTypeConfiguration<FloodReportDuplicateReference>
{
    public void Configure(EntityTypeBuilder<FloodReportDuplicateReference> builder)
    {
        builder.HasKey(o => o.Id);

        // FK to the parent duplicate source record
        builder
            .Property<Guid>("FloodReportSourceDuplicateId")
            .IsRequired();

        // Referential integrity: FloodReportId must point to an existing FloodReport
        builder
            .HasOne<FloodReport>()
            .WithMany()
            .HasForeignKey(o => o.FloodReportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(o => o.FloodReportId);
    }
}