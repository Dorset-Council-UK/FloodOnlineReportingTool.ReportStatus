using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodReportSourceOrganisationConfiguration : IEntityTypeConfiguration<FloodReportSourceOrganisation>
{
    public void Configure(EntityTypeBuilder<FloodReportSourceOrganisation> builder)
    {
        builder.HasKey(org => new { org.FloodReportId, org.FloodReportSourceId, org.OrganisationId, org.FloodAuthorityId });

        builder.Property(org => org.OrganisationName).IsRequired().HasMaxLength(200);
        builder.Property(org => org.FloodAuthorityName).IsRequired().HasMaxLength(200);
    }
}