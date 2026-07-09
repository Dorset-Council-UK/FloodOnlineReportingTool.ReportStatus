namespace FloodOnlineReportingTool.ReportStatus.Endpoints.Checking;

internal sealed record ImportValidationResult(int TotalRecords, int NumberOfIssues, IReadOnlyCollection<ImportValidationIssue> Issues);
