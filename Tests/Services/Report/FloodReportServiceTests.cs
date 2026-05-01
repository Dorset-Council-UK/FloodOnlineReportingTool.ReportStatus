using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using NSubstitute;
using Tests.Data;

namespace Tests.Services.Report;

public class FloodReportServiceTests
{
    // All tests use the single shared dataset — mirrors the real FloodReports table
    // containing many unrelated records, so tests that return too many results will fail.
    private static FloodReportService CreateService()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var floodReportRepository = Substitute.For<IFloodReportRepository>();
        var floodReportSourceRepository = Substitute.For<IFloodReportSourceDuplicateRepository>();
        var context = Substitute.For<ApplicationDbContext>(options);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
            {
                ["ReportStatus:LocationBufferMetres"] = "25",
                ["ReportStatus:GroundwaterCapDays"] = "31",
                ["ReportStatus:FloodingCapDays"] = "14",
            })
            .Build();

        var mockDbSet = FindPotentialDuplicatesData.AllReports.BuildMockDbSet();
        context.FloodReports.Returns(mockDbSet);

        return new FloodReportService(logger, floodReportRepository, configuration, floodReportSourceRepository, context);
    }

    // --- Property (UPRN) ---

    // Scenario 1: Exact date match, same UPRN, same authority — ID must be in results
    // Scenario 2: UPRN match, existing event within 7-day buffer window — ID must be in results
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenPropertyCriteriaMet(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid expectedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.Contains(expectedId, result);
    }

    // Scenario 3: Different UPRN — must not be returned
    // Scenario 4: Correct UPRN, event too early (ended outside 7-day buffer) — must not be returned
    // Scenario 5: Correct UPRN, event too late (starts after incoming ends) — must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenPropertyCriteriaNotMet(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid excludedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.DoesNotContain(excludedId, result);
    }

    // Scenario 6: Same UPRN + date, same authority (EA) — report must be returned
    // Scenario 7: Same UPRN + date, different authority (LLFA vs EA) — report must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyAuthority_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatchOrNoMatch_WhenPropertyAuthorityMatchesOrDiffers(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid reportId, bool shouldBeContained)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        if (shouldBeContained)
            Assert.Contains(reportId, result);
        else
            Assert.DoesNotContain(reportId, result);
    }

    // Scenario 8: Two reports matching UPRN + authority + date window — both IDs must be in results
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyMultipleMatches_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMultipleMatches_WhenMultiplePropertyReportsMatch(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid[] expectedIds)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        foreach (var id in expectedIds)
            Assert.Contains(id, result);
    }

    // Scenario 9: Ongoing incoming — DB record within the cap window must be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyOngoing_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenPropertyIncomingEventIsOngoing(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid expectedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.Contains(expectedId, result);
    }

    // Scenario 31: Ongoing incoming but DB record starts beyond FloodingCapDays — must not be returned
    // Scenario 32: Historical DB record marked as ongoing — DB-side cap prevents effective end reaching UtcNow — must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyOngoingNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenPropertyOngoingCapExceeded(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid excludedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.DoesNotContain(excludedId, result);
    }

    // --- Road (USRN) ---

    // Scenario 10: Exact date match, same USRN, same authority — ID must be in results
    // Scenario 11: USRN match, existing event within 7-day buffer window — ID must be in results
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenRoadCriteriaMet(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid expectedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.Contains(expectedId, result);
    }

    // Scenario 12: Different USRN — must not be returned
    // Scenario 13: Correct USRN, event too early (outside 7-day buffer) — must not be returned
    // Scenario 14: Correct USRN, event too late (starts after incoming ends) — must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenRoadCriteriaNotMet(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid excludedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.DoesNotContain(excludedId, result);
    }

    // Scenario 15: Same USRN + date, same authority — report must be returned
    // Scenario 16: Same USRN + date, different authority — report must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadAuthority_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatchOrNoMatch_WhenRoadAuthorityMatchesOrDiffers(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid reportId, bool shouldBeContained)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        if (shouldBeContained)
            Assert.Contains(reportId, result);
        else
            Assert.DoesNotContain(reportId, result);
    }

    // Scenario 17: Two reports matching USRN + authority + date window — both IDs must be in results
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadMultipleMatches_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMultipleMatches_WhenMultipleRoadReportsMatch(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid[] expectedIds)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        foreach (var id in expectedIds)
            Assert.Contains(id, result);
    }

    // Scenario 18: Ongoing incoming — DB record within the cap window must be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadOngoing_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenRoadIncomingEventIsOngoing(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid expectedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.Contains(expectedId, result);
    }

    // Scenario 33: Ongoing incoming but DB record starts beyond FloodingCapDays — must not be returned
    // Scenario 34: Historical DB record marked as ongoing — DB-side cap prevents effective end reaching UtcNow — must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadOngoingNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenRoadOngoingCapExceeded(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid excludedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.DoesNotContain(excludedId, result);
    }

    // --- Location (geometry buffer) ---

    // Scenario 19: Existing report 20m away (within 25m buffer), same authority, overlapping date — must be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenLocationWithinBuffer(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, double buffer, Guid expectedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, buffer, incomingRecord, TestContext.Current.CancellationToken);
        Assert.Contains(expectedId, result);
    }

    // Scenario 20: Existing report 30m away (outside 25m buffer) — must not be returned
    // Scenario 21: Within buffer but event too early (outside 7-day window) — must not be returned
    // Scenario 22: Within buffer but event too late (starts after incoming ends) — must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenLocationCriteriaNotMet(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, double buffer, Guid excludedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, buffer, incomingRecord, TestContext.Current.CancellationToken);
        Assert.DoesNotContain(excludedId, result);
    }

    // Scenario 23: Within buffer, same authority — must be returned
    // Scenario 24: Within buffer, different authority — must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationAuthority_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatchOrNoMatch_WhenLocationAuthorityMatchesOrDiffers(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, double buffer, Guid reportId, bool shouldBeContained)
    {
        var result = await CreateService().FindPotentialDuplicates(type, buffer, incomingRecord, TestContext.Current.CancellationToken);
        if (shouldBeContained)
            Assert.Contains(reportId, result);
        else
            Assert.DoesNotContain(reportId, result);
    }

    // Scenario 25: Two reports within 25m buffer, same authority, different dates — both IDs must be in results
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationMultipleMatches_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMultipleMatches_WhenMultipleLocationReportsWithinBuffer(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, double buffer, Guid[] expectedIds)
    {
        var result = await CreateService().FindPotentialDuplicates(type, buffer, incomingRecord, TestContext.Current.CancellationToken);
        foreach (var id in expectedIds)
            Assert.Contains(id, result);
    }

    // --- Groundwater date buffer ---

    // Scenario 26: Property, groundwater incoming — 14-day buffer applied — report 10 days ago must be returned
    // Scenario 27: Road, groundwater incoming — 14-day buffer applied — report 10 days ago must be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.GroundwaterBuffer_ReturnsMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenGroundwaterIncomingUsesExtendedDateBuffer(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid expectedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.Contains(expectedId, result);
    }

    // Scenario 28: Property, river incoming — 7-day buffer applied — report 10 days ago must not be returned
    // Scenario 29: Road, river incoming — 7-day buffer applied — report 10 days ago must not be returned
    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.GroundwaterBuffer_ReturnsNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenNonGroundwaterIncomingUsesStandardDateBuffer(string _, FloodReportType type, EligibilityCheckRecord incomingRecord, Guid excludedId)
    {
        var result = await CreateService().FindPotentialDuplicates(type, 0, incomingRecord, TestContext.Current.CancellationToken);
        Assert.DoesNotContain(excludedId, result);
    }

    // Scenario 30: Two reports in AllReports — verifies the matching ID is returned and the non-matching ID is excluded
    [Fact]
    public async Task FindPotentialDuplicates_ReturnsCorrectIds_NotJustCount()
    {
        var incomingRecord = FindPotentialDuplicatesData.CreateIncomingRecord(uprn: 10023242411);
        var result = await CreateService().FindPotentialDuplicates(FloodReportType.Property, 0, incomingRecord, TestContext.Current.CancellationToken);

        Assert.Contains(FindPotentialDuplicatesData.Property_ExactDateMatch.Id, result);
        Assert.DoesNotContain(FindPotentialDuplicatesData.Property_DifferentUprn.Id, result);
    }
}
