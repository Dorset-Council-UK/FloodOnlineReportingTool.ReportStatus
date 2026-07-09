using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints.Checking;

internal sealed class ImportFloodEventValidation
{
    private const string EntityName = nameof(FloodEvent);

    internal static async Task<Results<Ok<ImportValidationResult>, BadRequest<string>>> ImportValidateFloodEvents(IHostEnvironment environment, ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (environment.IsProduction())
        {
            return TypedResults.BadRequest("Import validation is not allowed in production.");
        }

        var floodEvents = await context.FloodEvents
            .OrderBy(fe => fe.CreatedUtc) // oldest first
            .ToListAsync(cancellationToken);

        if (floodEvents.Count == 0)
        {
            return TypedResults.Ok(new ImportValidationResult(0, 0, []));
        }

        var issues = new List<ImportValidationIssue>();
        string reason = string.Empty;

        foreach (var floodEvent in floodEvents)
        {
            if (!IsValidId(floodEvent, out var issue))
            {
                issues.Add(issue);
            }

            if (!IsValidReference(floodEvent, context, out issue))
            {
                issues.Add(issue);
            }

            //if (!IsValidTitle(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //if (!IsValidRecordStatus(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //if (!IsValidFloodStatus(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //if (!IsValidInvestigationStatus(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //if (!IsValidCreatedDate(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //if (!IsValidUpdatedDate(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //foreach (var linkedFloodReport in floodEvent.LinkedFloodReports)
            //{
            //    if (!IsValidLinkedFloodReport(linkedFloodReport, out issue))
            //    {
            //        issues.Add(issue);
            //    }
            //}

            //if (!IsValidGeom(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //if (!IsValidFloodStartDate(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //if (!IsValidFloodEndDate(floodEvent, out issue))
            //{
            //    issues.Add(issue);
            //}

            //foreach (var floodCause in floodEvent.FloodCauses)
            //{
            //    if (!IsValidFloodCause(floodCause, out issue))
            //    {
            //        issues.Add(issue);
            //    }
            //}

            //foreach (var organisation in floodEvent.Organisations)
            //{
            //    if (!IsValidOrganisation(organisation, out issue))
            //    {
            //        issues.Add(issue);
            //    }
            //}

            //foreach (var statusFilter in floodEvent.StatusFilter)
            //{
            //    if (!IsValidStatusFilter(statusFilter, out issue))
            //    {
            //        issues.Add(issue);
            //    }
            //}
        }

        var result = new ImportValidationResult(floodEvents.Count, issues.Count, issues);
        return TypedResults.Ok(result);
    }

    private static ImportValidationIssue? CheckId(FloodEvent floodEvent)
    {
        const string FieldName = nameof(FloodEvent.Id);

        if (floodEvent.Id == null)
        {
            return new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid GUID: value is null.");
        }

        if (floodEvent.Id == Guid.Empty)
        {
            return new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid GUID: value is an empty GUID.");
        }

        return null;
    }

    private static bool IsValidId(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.Id);

        if (floodEvent.Id == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid GUID: value is null.");
            return false;
        }

        if (floodEvent.Id == Guid.Empty)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid GUID: value is an empty GUID.");
            return false;
        }

        issue = null;
        return true;
    }

    private static bool IsValidReference(FloodEvent floodEvent, ApplicationDbContext context, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.Reference);

        if (floodEvent.Reference == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid reference: value is null.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(floodEvent.Reference))
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid reference: value is empty or whitespace.");
            return false;
        }

        // does the reference already exist in the database?
        var alreadyExists = context.FloodEvents.Any(fe => fe.Reference == floodEvent.Reference && fe.Id != floodEvent.Id);
        if (alreadyExists)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid reference: value '{floodEvent.Reference}' already exists in the database.");
            return false;
        }

        if (floodEvent.Reference.Length < 5)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid reference: value '{floodEvent.Reference}' is too short.");
            return false;
        }

        if (floodEvent.Reference.Length > 10)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"{EntityName}.{FieldName} is not a valid reference: value '{floodEvent.Reference}' is too long.");
            return false;
        }

        issue = null;
        return true;
    }
}