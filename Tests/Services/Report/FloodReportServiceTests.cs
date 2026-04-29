using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Database.DbContexts;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Repositories;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.NSubstitute;
using NSubstitute;
using Tests.Data;

namespace Tests.Services.Report;

public class FloodReportServiceTests
{
    private static FloodReportService CreateService(ICollection<FloodReport> reports)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
        var logger = Substitute.For<ILogger<FloodReportService>>();
        var floodReportRepository = Substitute.For<IFloodReportRepository>();
        var floodReportSourceRepository = Substitute.For<IFloodReportSourceDuplicateRepository>();
        var context = Substitute.For<ApplicationDbContext>(options);

        var mockDbSet = reports.BuildMockDbSet();
        context.FloodReports.Returns(mockDbSet);

        return new FloodReportService(logger, floodReportRepository, floodReportSourceRepository, context);
    }

    // --- Property (UPRN) ---

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenPropertyCriteriaMet(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenPropertyCriteriaNotMet(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyAuthorityMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatchOrNoMatch_WhenPropertyAuthorityMatchesOrDiffers(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyMultipleMatches_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMultipleMatches_WhenMultiplePropertyReportsMatch(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.PropertyOngoing_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenPropertyIncomingEventIsOngoing(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    // --- Road (USRN) ---

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenRoadCriteriaMet(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenRoadCriteriaNotMet(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadAuthorityMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatchOrNoMatch_WhenRoadAuthorityMatchesOrDiffers(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadMultipleMatches_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMultipleMatches_WhenMultipleRoadReportsMatch(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.RoadOngoing_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenRoadIncomingEventIsOngoing(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    // --- Location (geometry buffer) ---

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenLocationWithinBuffer(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, double buffer, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, buffer, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenLocationCriteriaNotMet(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, double buffer, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, buffer, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationAuthorityMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatchOrNoMatch_WhenLocationAuthorityMatchesOrDiffers(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, double buffer, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, buffer, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.LocationMultipleMatches_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMultipleMatches_WhenMultipleLocationReportsWithinBuffer(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, double buffer, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, buffer, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    // --- Groundwater date buffer ---

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.GroundwaterBuffer_ReturnsMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsMatch_WhenGroundwaterIncomingUsesExtendedDateBuffer(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    [Theory]
    [MemberData(nameof(FindPotentialDuplicatesData.GroundwaterBuffer_ReturnsNoMatch_Cases), MemberType = typeof(FindPotentialDuplicatesData))]
    public async Task FindPotentialDuplicates_ReturnsNoMatch_WhenNonGroundwaterIncomingUsesStandardDateBuffer(FloodReport[] reports, FloodReportType type, EligibilityCheckRecord ecr, int expected)
    {
        var result = await CreateService(reports).FindPotentialDuplicates(type, 0, ecr, TestContext.Current.CancellationToken);
        Assert.Equal(expected, result.Count);
    }

    // --- ID correctness ---

    [Fact]
    public async Task FindPotentialDuplicates_ReturnsCorrectIds_NotJustCount()
    {
        var expectedReport = FindPotentialDuplicatesData.CreateReport(uprn: 10023242411);
        var unrelatedReport = FindPotentialDuplicatesData.CreateReport(uprn: 99999999999);
        var ecr = FindPotentialDuplicatesData.CreateEcr(uprn: 10023242411);

        var result = await CreateService([expectedReport, unrelatedReport])
            .FindPotentialDuplicates(FloodReportType.Property, 0, ecr, TestContext.Current.CancellationToken);

        Assert.Single(result);
        Assert.Contains(expectedReport.Id, result);
    }
}
