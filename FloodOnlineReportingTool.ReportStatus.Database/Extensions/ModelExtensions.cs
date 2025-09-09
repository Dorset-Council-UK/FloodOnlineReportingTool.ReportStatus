#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.ReportStatus.Database.Models;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class ModelExtensions
{
    public static ReportDto ToDto(this Report report)
    {
        return new()
        {
            EligibilityCheckId = report.EligibilityCheckId,
            Uprn = report.Uprn,
            Usrn = report.Usrn,
            Easting = report.Easting,
            Northing = report.Northing,
            ImpactStartUtc = report.ImpactStartUtc,
            ImpactDurationHours = report.ImpactDurationHours,
        };
    }

    internal static ReportCreated ToMessageCreated(this Report report)
    {
        return new(
            report.Id,
            report.EligibilityCheckId,
            report.CreatedUtc
        );
    }

    public static ReportDuplicateDto ToDto(this ReportDuplicate duplicate)
    {
        return new()
        {
            EligibilityCheckId = duplicate.EligibilityCheckId,
            Easting = duplicate.Easting,
            Northing = duplicate.Northing,
        };
    }

    internal static ReportDuplicateCreated ToMessageCreated(this ReportDuplicate duplicate)
    {
        return new(
            duplicate.Id,
            duplicate.ReportId,
            duplicate.EligibilityCheckId,
            duplicate.CreatedUtc
        );
    }

    public static ReportSourceDto ToDto(this ReportSource reportSource)
    {
        return new()
        {
            Name = reportSource.Name,
        };
    }
}
