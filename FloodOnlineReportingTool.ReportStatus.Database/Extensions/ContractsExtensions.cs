using FloodOnlineReportingTool.ReportStatus.Database.Models;

namespace FloodOnlineReportingTool.Contracts;

internal static class ContractsExtensions
{
    internal static ReportDuplicateDto ToReportDuplicateDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        return new()
        {
            EligibilityCheckId = eligibilityCheckCreated.Id,
            Easting = eligibilityCheckCreated.Easting,
            Northing = eligibilityCheckCreated.Northing,
        };
    }

    internal static ReportDto ToReportDto(this EligibilityCheckCreated eligibilityCheckCreated)
    {
        return new()
        {
            EligibilityCheckId = eligibilityCheckCreated.Id,
            Uprn = eligibilityCheckCreated.Uprn,
            Easting = eligibilityCheckCreated.Easting,
            Northing = eligibilityCheckCreated.Northing,
            ImpactStartUtc = eligibilityCheckCreated.ImpactStartUTC,
            ImpactDurationHours = eligibilityCheckCreated.ImpactDurationHours,
        };
    }
}
