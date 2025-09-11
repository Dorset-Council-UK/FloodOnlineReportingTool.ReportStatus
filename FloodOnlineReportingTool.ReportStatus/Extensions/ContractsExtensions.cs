using FloodOnlineReportingTool.ReportStatus.Database.Models;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FloodOnlineReportingTool.Contracts;
#pragma warning restore IDE0130 // Namespace does not match folder structure

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
            Usrn = eligibilityCheckCreated.Usrn,
            Easting = eligibilityCheckCreated.Easting,
            Northing = eligibilityCheckCreated.Northing,
            ImpactStartUtc = eligibilityCheckCreated.ImpactStartUTC,
            ImpactDurationHours = eligibilityCheckCreated.ImpactDurationHours,
        };
    }
}
