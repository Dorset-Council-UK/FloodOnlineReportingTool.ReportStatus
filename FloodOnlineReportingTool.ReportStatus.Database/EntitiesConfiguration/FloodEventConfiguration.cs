using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class FloodEventConfiguration : IEntityTypeConfiguration<FloodEvent>
{
    public void Configure(EntityTypeBuilder<FloodEvent> builder)
    {
        builder.HasKey(o => o.Id);
        // Unique constraint ensures Reference is consistent across FORT modules
        builder.HasAlternateKey(o => o.Reference);
    }  
};
