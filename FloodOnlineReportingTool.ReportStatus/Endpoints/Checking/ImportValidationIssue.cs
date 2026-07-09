namespace FloodOnlineReportingTool.ReportStatus.Endpoints.Checking;

internal sealed record ImportValidationIssue(string EntityName, string FieldName, string IssueDescription);
