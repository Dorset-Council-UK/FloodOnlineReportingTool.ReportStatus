using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceOrganisationConfiguration : IEntityTypeConfiguration<FloodReportSourceOrganisation>
{
    public void Configure(EntityTypeBuilder<FloodReportSourceOrganisation> builder)
    {
        builder.HasKey(o => o.Id);

        builder.HasIndex(o => new { o.FloodReportSourceId, o.OrganisationId, o.FloodAuthorityId }).IsUnique();

        builder.Property(org => org.OrganisationName).IsRequired().HasMaxLength(200);
        builder.Property(org => org.FloodAuthorityName).IsRequired().HasMaxLength(200);
    }
}