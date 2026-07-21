using FloodOnlineReportingTool.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.Database.EntitiesConfiguration;

internal class FloodReportSourceOrganisationConfiguration : IEntityTypeConfiguration<FloodReportSourceOrganisation>
{
    public void Configure(EntityTypeBuilder<FloodReportSourceOrganisation> builder)
    {
        builder.HasKey(o => o.Id);

        // Enforces uniqueness only when owned by a FloodReportSource
        builder.HasIndex(o => new { o.FloodReportSourceRef, o.OrganisationId, o.FloodAuthorityId })
            .IsUnique()
            .HasFilter("\"FloodReportSourceRef\" IS NOT NULL");

        // Enforces uniqueness only when owned by a FloodReportSourceDuplicate
        builder.HasIndex(o => new { o.FloodReportSourceDuplicateId, o.OrganisationId, o.FloodAuthorityId })
            .IsUnique()
            .HasFilter("\"FloodReportSourceDuplicateId\" IS NOT NULL");

        builder.Property(org => org.OrganisationName).IsRequired().HasMaxLength(200);
        builder.Property(org => org.FloodAuthorityName).IsRequired().HasMaxLength(200);

        builder.HasOne(x => x.FloodReportSource)
            .WithMany(x => x.Organisations)
            .HasForeignKey(x => x.FloodReportSourceRef)
            .HasPrincipalKey(x => x.Reference)
            .IsRequired(false);

        builder.HasOne(x => x.FloodReportSourceDuplicate)
            .WithMany(x => x.Organisations)
            .HasForeignKey(x => x.FloodReportSourceDuplicateId)
            .IsRequired(false);
    }
}