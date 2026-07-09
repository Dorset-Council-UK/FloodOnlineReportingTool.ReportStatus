using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace FloodOnlineReportingTool.ReportStatus.Endpoints.Checking;

internal sealed class ImportFloodEventValidation
{
    private const string EntityName = nameof(FloodEvent);

#pragma warning disable MA0051 // Method is too long
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

            if (!IsValidTitle(floodEvent, context, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidRecordStatus(floodEvent, context, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidFloodStatus(floodEvent, context, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidInvestigationStatus(floodEvent, context, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidCreatedDate(floodEvent, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidUpdatedDate(floodEvent, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidLinkedFloodReports(floodEvent, context, out var linkedIssues))
            {
                issues.AddRange(linkedIssues);
            }

            if (!IsValidGeom(floodEvent, context, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidFloodStartDate(floodEvent, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidFloodEndDate(floodEvent, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidFloodCauses(floodEvent, out var floodCausedIssues))
            {
                issues.AddRange(floodCausedIssues);
            }

            if (!IsValidOrganisations(floodEvent, context, out var organisationIssues))
            {
                issues.AddRange(organisationIssues);
            }

            if (!IsValidStatusFilters(floodEvent, context, out var statusFilterIssues))
            {
                issues.AddRange(statusFilterIssues);
            }
        }

        var result = new ImportValidationResult(floodEvents.Count, issues.Count, issues);
        return TypedResults.Ok(result);
    }

    private static bool IsValidId(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.Id);

        if (floodEvent.Id == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (floodEvent.Id == Guid.Empty)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is empty.");
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
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(floodEvent.Reference))
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is empty or whitespace.");
            return false;
        }

        // does the reference already exist in the database?
        var alreadyExists = context.FloodEvents
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Any(fe => fe.Reference == floodEvent.Reference && fe.Id != floodEvent.Id);
        if (alreadyExists)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.Reference}' already exists in the database.");
            return false;
        }

        if (floodEvent.Reference.Length < 5)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.Reference}' is too short.");
            return false;
        }

        if (floodEvent.Reference.Length > 10)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.Reference}' is too long.");
            return false;
        }

        issue = null;
        return true;
    }

    private static bool IsValidTitle(FloodEvent floodEvent, ApplicationDbContext context, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.Title);

        if (floodEvent.Title == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(floodEvent.Title))
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is empty or whitespace.");
            return false;
        }

        if (floodEvent.Title.Length < 5)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.Title}' is too short.");
            return false;
        }

        // check if other flood events have the same title
        var sameTitlesExist = context.FloodEvents
            .AsNoTracking()
            .IgnoreAutoIncludes()
            .Any(fe => fe.Title == floodEvent.Title && fe.Id != floodEvent.Id);
        if (sameTitlesExist)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.Title}' is used on other flood events in the database.");
            return false;
        }

        issue = null;
        return true;
    }

    private static bool IsValidRecordStatus(FloodEvent floodEvent, ApplicationDbContext context, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.RecordStatus);

        if (floodEvent.RecordStatus == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (floodEvent.RecordStatus == Guid.Empty)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is empty.");
            return false;
        }

        issue = null;
        return true;
    }

    private static bool IsValidFloodStatus(FloodEvent floodEvent, ApplicationDbContext context, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.FloodStatus);
        throw new NotImplementedException();
    }

    private static bool IsValidInvestigationStatus(FloodEvent floodEvent, ApplicationDbContext context, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.InvestigationStatus);
        throw new NotImplementedException();
    }

    private static bool IsValidCreatedDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.CreatedUtc);
        throw new NotImplementedException();
    }

    private static bool IsValidUpdatedDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.UpdatedUtc);
        throw new NotImplementedException();
    }

    private static bool IsValidLinkedFloodReports(FloodEvent floodEvent, ApplicationDbContext context, out ImportValidationIssue[] issues)
    {
        const string BaseFieldName = nameof(FloodEvent.LinkedFloodReports);
        throw new NotImplementedException();
    }

    private static bool IsValidGeom(FloodEvent floodEvent, ApplicationDbContext context, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.Geom);
        throw new NotImplementedException();
    }

    private static bool IsValidFloodStartDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.FloodStartDate);
        throw new NotImplementedException();
    }

    private static bool IsValidFloodEndDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.FloodEndDate);
        throw new NotImplementedException();
    }

    private static bool IsValidFloodCauses(FloodEvent floodEvent, out ICollection<ImportValidationIssue> issues)
    {
        const string BaseFieldName = nameof(FloodEvent.FloodCauses);

        if (floodEvent.FloodCauses == null)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: Flood causes is null.")];
            return false;
        }

        if (floodEvent.FloodCauses.Count == 0)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: There are no flood causes.")];
            return false;
        }

        issues = [];
        for (int i = 0; i < floodEvent.FloodCauses.Count; i++)
        {
            string fieldName = $"{BaseFieldName}[{i}]";
            var floodCauseId = floodEvent.FloodCauses.ElementAt(i);
            
            if (floodCauseId == null)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: Flood cause is null."));
            }

            if (floodCauseId == Guid.Empty)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: Flood cause is empty."));
            }
        }
        return issues.Count == 0;
    }

    private static bool IsValidOrganisations(FloodEvent floodEvent, ApplicationDbContext context, out ImportValidationIssue[] issues)
    {
        const string BaseFieldName = nameof(FloodEvent.Organisations);
        throw new NotImplementedException();
    }

    private static bool IsValidStatusFilters(FloodEvent floodEvent, ApplicationDbContext context, out ImportValidationIssue[] issues)
    {
        const string BaseFieldName = nameof(FloodEvent.StatusFilter);
        throw new NotImplementedException();
    })
}