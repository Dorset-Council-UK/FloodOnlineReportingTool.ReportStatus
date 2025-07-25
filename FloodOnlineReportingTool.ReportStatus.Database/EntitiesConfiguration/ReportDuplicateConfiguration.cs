using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FloodOnlineReportingTool.ReportStatus.Database.EntitiesConfiguration;

internal class ReportDuplicateConfiguration : IEntityTypeConfiguration<ReportDuplicate>
{
    public void Configure(EntityTypeBuilder<ReportDuplicate> builder)
    {
        builder
            .Property(o => o.Id)
            .ValueGeneratedNever();
    }
}
