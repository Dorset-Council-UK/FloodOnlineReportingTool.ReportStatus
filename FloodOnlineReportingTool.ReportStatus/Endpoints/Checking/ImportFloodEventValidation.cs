using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
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

            if (!IsValidRecordStatus(floodEvent, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidFloodStatus(floodEvent, out issue))
            {
                issues.Add(issue);
            }

            if (!IsValidInvestigationStatus(floodEvent, out issue))
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

            if (!IsValidLinkedFloodReports(floodEvent, out var linkedIssues))
            {
                issues.AddRange(linkedIssues);
            }

            if (!IsValidGeom(floodEvent, out var geomIssues))
            {
                issues.AddRange(geomIssues);
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

            if (!IsValidOrganisations(floodEvent, out var organisationIssues))
            {
                issues.AddRange(organisationIssues);
            }

            if (!IsValidStatusFilters(floodEvent, out var statusFilterIssues))
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

    private static bool IsValidRecordStatus(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
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

        if (!RecordStatusIds.All.Contains(floodEvent.RecordStatus))
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.RecordStatus}' is not a valid flood cause.");
        }

        issue = null;
        return true;
    }

    private static bool IsValidFloodStatus(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.FloodStatus);

        if (floodEvent.FloodStatus == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (floodEvent.FloodStatus == Guid.Empty)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is empty.");
            return false;
        }

        if (!EventStatusIds.All.Contains(floodEvent.FloodStatus))
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.FloodStatus}' is not a valid flood status.");
        }

        issue = null;
        return true;
    }

    private static bool IsValidInvestigationStatus(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.InvestigationStatus);

        if (floodEvent.InvestigationStatus == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (floodEvent.InvestigationStatus == Guid.Empty)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is empty.");
            return false;
        }

        if (!Section19StatusIds.All.Contains(floodEvent.InvestigationStatus))
        {
            issue = new ImportValidationIssue(EntityName, FieldName, $"Not valid: '{floodEvent.InvestigationStatus}' is not a valid investigation status.");
        }

        issue = null;
        return true;
    }

    private static bool IsValidCreatedDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.CreatedUtc);

        if (floodEvent.CreatedUtc == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (floodEvent.CreatedUtc == DateTimeOffset.MinValue)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MinValue.");
            return false;
        }

        if (floodEvent.CreatedUtc == DateTimeOffset.MaxValue)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MaxValue.");
            return false;
        }

        if (floodEvent.CreatedUtc > DateTimeOffset.UtcNow)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is in the future.");
            return false;
        }

        if (floodEvent.UpdatedUtc != null && floodEvent.CreatedUtc > floodEvent.UpdatedUtc)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: created after it was updated.");
            return false;
        }

        issue = null;
        return true;
    }

    private static bool IsValidUpdatedDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.UpdatedUtc);

        if (floodEvent.UpdatedUtc.HasValue)
        {
            var updatedUtc = floodEvent.UpdatedUtc.Value;

            if (updatedUtc == DateTimeOffset.MinValue)
            {
                issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MinValue.");
                return false;
            }

            if (updatedUtc == DateTimeOffset.MaxValue)
            {
                issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MaxValue.");
                return false;
            }

            if (updatedUtc > DateTimeOffset.UtcNow)
            {
                issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is in the future.");
                return false;
            }

            if (updatedUtc < floodEvent.CreatedUtc)
            {
                issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: updated before it was created.");
                return false;
            }
        }

        issue = null;
        return true;
    }

    private static bool IsValidLinkedFloodReports(FloodEvent floodEvent, out ICollection<ImportValidationIssue> issues)
    {
        const string BaseFieldName = nameof(FloodEvent.LinkedFloodReports);

        if (floodEvent.LinkedFloodReports == null)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is null.")];
            return false;
        }

        if (floodEvent.LinkedFloodReports.Count == 0)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is empty.")];
            return false;
        }

        issues = [];
        for (int i = 0; i < floodEvent.LinkedFloodReports.Count; i++)
        {
            string fieldName = $"{BaseFieldName}[{i}]";
            string linkedReport = floodEvent.LinkedFloodReports.ElementAt(i);

            if (linkedReport == null)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is null."));
            }

            if (string.IsNullOrWhiteSpace(linkedReport))
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is empty or whitespace."));
            }

            if (linkedReport?.Length < 5)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, $"Not valid: '{linkedReport}' is too short."));
                return false;
            }
        }
        return issues.Count == 0;
    }

    private static bool IsValidGeom(FloodEvent floodEvent, out ICollection<ImportValidationIssue> issues)
    {
        const string BaseFieldName = nameof(FloodEvent.Geom);

        if (floodEvent.Geom == null)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is null.")];
            return false;
        }

        if (floodEvent.Geom.Count == 0)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is empty.")];
            return false;
        }

        issues = [];
        for (int i = 0; i < floodEvent.Geom.Count; i++)
        {
            string fieldName = $"{BaseFieldName}.Coordinates[{i}]";
            var coordinate = floodEvent.Geom.Coordinates[i];

            if (coordinate == null)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is null."));
            }

            if (coordinate?.X == 0)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: X is zero."));
            }

            if (coordinate?.Y == 0)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: Y is zero."));
            }

            if (coordinate?.Z == 0)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: Z is zero."));
            }
        }
        return issues.Count == 0;
    }

    private static bool IsValidFloodStartDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.FloodStartDate);

        if (floodEvent.FloodStartDate == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (floodEvent.FloodStartDate == DateTimeOffset.MinValue)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MinValue.");
            return false;
        }

        if (floodEvent.FloodStartDate == DateTimeOffset.MaxValue)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MaxValue.");
            return false;
        }

        if (floodEvent.FloodStartDate > DateTimeOffset.UtcNow)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is in the future.");
            return false;
        }

        if (floodEvent.FloodEndDate != null && floodEvent.FloodStartDate > floodEvent.FloodEndDate)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: flood started after it ended.");
            return false;
        }

        issue = null;
        return true;
    }

    private static bool IsValidFloodEndDate(FloodEvent floodEvent, [NotNullWhen(false)] out ImportValidationIssue? issue)
    {
        const string FieldName = nameof(FloodEvent.FloodEndDate);

        if (floodEvent.FloodEndDate == null)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is null.");
            return false;
        }

        if (floodEvent.FloodEndDate == DateTimeOffset.MinValue)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MinValue.");
            return false;
        }

        if (floodEvent.FloodEndDate == DateTimeOffset.MaxValue)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is MaxValue.");
            return false;
        }

        if (floodEvent.FloodEndDate > DateTimeOffset.UtcNow)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: is in the future.");
            return false;
        }

        if (floodEvent.FloodStartDate != null && floodEvent.FloodEndDate < floodEvent.FloodStartDate)
        {
            issue = new ImportValidationIssue(EntityName, FieldName, "Not valid: flood ended before it started.");
            return false;
        }

        issue = null;
        return true;
    }

    private static bool IsValidFloodCauses(FloodEvent floodEvent, out ICollection<ImportValidationIssue> issues)
    {
        const string BaseFieldName = nameof(FloodEvent.FloodCauses);

        if (floodEvent.FloodCauses == null)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is null.")];
            return false;
        }

        if (floodEvent.FloodCauses.Count == 0)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is empty.")];
            return false;
        }

        ImmutableHashSet<Guid> allIds = [.. PrimaryCauseIds.All, .. SecondaryCauseIds.All];
        issues = [];
        for (int i = 0; i < floodEvent.FloodCauses.Count; i++)
        {
            string fieldName = $"{BaseFieldName}[{i}]";
            var floodCauseId = floodEvent.FloodCauses.ElementAt(i);
            
            if (floodCauseId == null)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is null."));
            }

            if (floodCauseId == Guid.Empty)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is empty."));
            }

            if (!allIds.Contains(floodCauseId))
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, $"Not valid: '{floodCauseId}' is not a valid flood cause."));
            }
        }
        return issues.Count == 0;
    }

    private static bool IsValidOrganisations(FloodEvent floodEvent, out ICollection<ImportValidationIssue> issues)
    {
        const string BaseFieldName = nameof(FloodEvent.Organisations);

        if (floodEvent.Organisations == null)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is null.")];
            return false;
        }

        if (floodEvent.Organisations.Count == 0)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is empty.")];
            return false;
        }

        issues = [];
        for (int i = 0; i < floodEvent.Organisations.Count; i++)
        {
            string fieldName = $"{BaseFieldName}[{i}]";
            var organisationId = floodEvent.Organisations.ElementAt(i);

            if (organisationId == null)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is null."));
            }

            if (organisationId == Guid.Empty)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is empty."));
            }

            if (!OrganisationIds.All.Contains(organisationId))
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, $"Not valid: '{organisationId}' is not a valid organisation."));
            }
        }
        return issues.Count == 0;
    }

    private static bool IsValidStatusFilters(FloodEvent floodEvent, out ICollection<ImportValidationIssue> issues)
    {
        const string BaseFieldName = nameof(FloodEvent.StatusFilter);

        if (floodEvent.StatusFilter == null)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is null.")];
            return false;
        }

        if (floodEvent.StatusFilter.Count == 0)
        {
            issues = [new ImportValidationIssue(EntityName, BaseFieldName, "Not valid: is empty.")];
            return false;
        }

        issues = [];
        for (int i = 0; i < floodEvent.StatusFilter.Count; i++)
        {
            string fieldName = $"{BaseFieldName}[{i}]";
            var statusFilterId = floodEvent.StatusFilter.ElementAt(i);

            if (statusFilterId == null)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is null."));
            }

            if (statusFilterId == Guid.Empty)
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, "Not valid: is empty."));
            }

            if (!EventStatusIds.All.Contains(statusFilterId))
            {
                issues.Add(new ImportValidationIssue(EntityName, fieldName, $"Not valid: '{statusFilterId}' is not a valid status filter."));
            }
        }
        return issues.Count == 0;
    }
}