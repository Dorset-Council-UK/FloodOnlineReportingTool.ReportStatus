using Bogus;
using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.ReportStatus.Consumers;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Services;
using FloodOnlineReportingTool.ReportStatus.Settings;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Tests.Helpers;
using static FloodOnlineReportingTool.ReportStatus.Database.Models.FloodReport;

namespace Tests.Consumers;

public class EligibilityCheckCreatedConsumerTests
{
    public static IEnumerable<TheoryDataRow<EligibilityCheckCreated>> GoodMessages => EligibilityCheckCreatedTestHelpers.CreateMessages(100);

    [Theory]
    [MemberData(nameof(GoodMessages))]
    public async Task Consume_ShouldCreateReport_WhenNotDuplicate(EligibilityCheckCreated message)
    {
        // Arrange
        var logger = Substitute.For<ILogger<EligibilityCheckCreatedConsumer>>();
        var settings = Substitute.For<IOptions<ReportStatusSettings>>();
        var reportService = Substitute.For<IFloodReportService>();
        var floodReportSourceService = Substitute.For<IFloodReportSourceService>();
        var floodPropertyService = Substitute.For<IFloodPropertyService>();
        var floodRoadService = Substitute.For<IFloodRoadService>();
        var floodLocationService = Substitute.For<IFloodLocationService>();
        var floodPropertyFloodReportService = Substitute.For<IFloodPropertyFloodReportService>();
        var floodRoadFloodReportService = Substitute.For<IFloodRoadFloodReportService>();
        var floodLocationFloodReportService = Substitute.For<IFloodLocationFloodReportService>();

        var consumer = new EligibilityCheckCreatedConsumer(logger, settings, reportService,floodReportSourceService, floodPropertyService, floodRoadService, floodLocationService, floodPropertyFloodReportService, floodRoadFloodReportService, floodLocationFloodReportService);
        var consumeContext = Substitute.For<ConsumeContext<EligibilityCheckCreated>>();
        consumeContext.Message.Returns(message);
        consumeContext.CancellationToken.Returns(CancellationToken.None);

        // All duplicate checks will return null, so it's not a duplicate
        floodPropertyService.GetFloodPropertiesByUprn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);
        floodRoadService.GetFloodRoadsByUsrn(Arg.Any<long>(), Arg.Any<CancellationToken>()).Returns([]);
        floodLocationService.GetFloodLocationsByPoint(Arg.Any<double>(), Arg.Any<double>(), Arg.Any<double>(), Arg.Any<CancellationToken>()).Returns([]);

        // Act
        await consumer.Consume(consumeContext);

        // Assert
        await reportService.Received(1).CreateFloodReport(message.ToFloodReportDto(), Arg.Any<CancellationToken>());
        //await reportDuplicateService.DidNotReceiveWithAnyArgs().CreateDuplicate(Arg.Any<Guid>(), Arg.Any<ReportDuplicateDto>(), Arg.Any<CancellationToken>());
    }

    //[Fact]
    //public async Task Consume_ShouldCreateDuplicate_WhenReportFoundByUprn()
    //{

    //}

    //[Fact]
    //public async Task Consume_ShouldCreateDuplicate_WhenReportFoundByUsrn()
    //{
    //}

    //[Fact]
    //public async Task Consume_ShouldCreateDuplicate_WhenReportIsNearby()
    //{
    //}
}
