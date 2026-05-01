using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceOrganisationConfiguration : IEntityTypeConfiguration<FloodReportSourceOrganisation>
{
    public void Configure(EntityTypeBuilder<FloodReportSourceOrganisation> builder)
    {
        builder.HasKey(o => o.Id);

        // Enforces uniqueness only when owned by a FloodReportSource
        builder.HasIndex(o => new { o.FloodReportSourceId, o.OrganisationId, o.FloodAuthorityId })
            .IsUnique()
            .HasFilter("\"FloodReportSourceId\" IS NOT NULL");

        // Enforces uniqueness only when owned by a FloodReportSourceDuplicate
        builder.HasIndex(o => new { o.FloodReportSourceDuplicateId, o.OrganisationId, o.FloodAuthorityId })
            .IsUnique()
            .HasFilter("\"FloodReportSourceDuplicateId\" IS NOT NULL");

        builder.Property(org => org.OrganisationName).IsRequired().HasMaxLength(200);
        builder.Property(org => org.FloodAuthorityName).IsRequired().HasMaxLength(200);

        builder.HasOne(x => x.FloodReportSource)
            .WithMany(x => x.Organisations)
            .HasForeignKey(x => x.FloodReportSourceId)
            .IsRequired(false);

        builder.HasOne(x => x.FloodReportSourceDuplicate)
            .WithMany(x => x.Organisations)
            .HasForeignKey(x => x.FloodReportSourceDuplicateId)
            .IsRequired(false);
    }
}