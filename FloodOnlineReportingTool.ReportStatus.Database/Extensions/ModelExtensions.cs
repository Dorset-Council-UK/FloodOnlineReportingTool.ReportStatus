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

    public static ReportDuplicateDto ToDto(this ReportDuplicate duplicate)
    {
        return new()
        {
            EligibilityCheckId = duplicate.EligibilityCheckId,
            Easting = duplicate.Easting,
            Northing = duplicate.Northing,
        };
    }
}
