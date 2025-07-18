namespace FloodOnlineReportingTool.ReportStatus.Database.Models;

public static class ModelExtensions
{
    public static ReportDto ToDto(this Report report)
    {
        return new()
        {
            EligibilityCheckId = report.EligibilityCheckId,
            Uprn = report.Uprn,
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
            ReportId = duplicate.ReportId,
            EligibilityCheckId = duplicate.EligibilityCheckId,
            Easting = duplicate.Easting,
            Northing = duplicate.Northing,
        };
    }
}
