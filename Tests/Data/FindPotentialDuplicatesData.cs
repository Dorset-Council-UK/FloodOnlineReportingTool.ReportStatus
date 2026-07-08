using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.Contracts.Shared.Models;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FindPotentialDuplicatesData
{
    private static readonly DateTimeOffset BaseImpactDate = new(2024, 12, 10, 10, 0, 0, TimeSpan.Zero);
    private static readonly double BaseEasting = 368991.12;
    private static readonly double BaseNorthing = 90881.94;

    // Fixed authority IDs — no FloodAuthorityIds constants exist in the contracts package
    internal static readonly Guid EnvironmentAgencyId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
    internal static readonly Guid LlfaId             = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002");

    // -------------------------------------------------------------------------
    // Named reports with fixed IDs so tests can assert on specific GUIDs.
    // Each name describes the ONE characteristic that makes it distinctive.
    // -------------------------------------------------------------------------

    // Property (UPRN) reports
    public static readonly FloodReport Property_ExactDateMatch =
        CreateReport(id: Guid.Parse("11111111-0001-0000-0000-000000000000"),
            uprn: 10023242411, impactStartUtc: BaseImpactDate);

    public static readonly FloodReport Property_WithinWindow =
        CreateReport(id: Guid.Parse("11111111-0002-0000-0000-000000000000"),
            uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1);

    public static readonly FloodReport Property_TooEarly =
        CreateReport(id: Guid.Parse("11111111-0003-0000-0000-000000000000"),
            uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-30), impactDurationHours: 1);

    public static readonly FloodReport Property_TooLate =
        CreateReport(id: Guid.Parse("11111111-0004-0000-0000-000000000000"),
            uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(30));

    public static readonly FloodReport Property_DifferentUprn =
        CreateReport(id: Guid.Parse("11111111-0005-0000-0000-000000000000"),
            uprn: 99999999999, impactStartUtc: BaseImpactDate);

    public static readonly FloodReport Property_DifferentAuthority =
        CreateReport(id: Guid.Parse("11111111-0006-0000-0000-000000000000"),
            uprn: 10023242411, impactStartUtc: BaseImpactDate, authorityId: LlfaId);

    public static readonly FloodReport Property_GroundwaterWindow =
        CreateReport(id: Guid.Parse("11111111-0007-0000-0000-000000000000"),
            uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-10), impactDurationHours: 1);

    // Matched only by an ongoing incoming — starts after the 24h normal duration end
    // but within the 14-day FloodingCapDays window (i.e. AddDays(3) < 14).
    public static readonly FloodReport Property_OngoingOnlyMatch =
        CreateReport(id: Guid.Parse("11111111-0008-0000-0000-000000000000"),
            uprn: 10023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(3));

    // Stale ongoing — starts beyond FloodingCapDays (21 > 14) so must never match,
    // even when the incoming record itself is ongoing.
    public static readonly FloodReport Property_OngoingCapExceeded =
        CreateReport(id: Guid.Parse("11111111-0009-0000-0000-000000000000"),
            uprn: 10023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(21));

    // Stale ongoing — starts before but should be capped so must never match,
    // even when the incoming record itself is ongoing.
    public static readonly FloodReport Property_OngoingHistorical =
        CreateReport(id: Guid.Parse("11111111-0010-0000-0000-000000000000"),
            uprn: 10023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(-100));

    // Road (USRN) reports
    public static readonly FloodReport Road_ExactDateMatch =
        CreateReport(id: Guid.Parse("22222222-0001-0000-0000-000000000000"),
            usrn: 20023242411, impactStartUtc: BaseImpactDate);

    public static readonly FloodReport Road_WithinWindow =
        CreateReport(id: Guid.Parse("22222222-0002-0000-0000-000000000000"),
            usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1);

    public static readonly FloodReport Road_TooEarly =
        CreateReport(id: Guid.Parse("22222222-0003-0000-0000-000000000000"),
            usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-30), impactDurationHours: 1);

    public static readonly FloodReport Road_TooLate =
        CreateReport(id: Guid.Parse("22222222-0004-0000-0000-000000000000"),
            usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(30));

    public static readonly FloodReport Road_DifferentUsrn =
        CreateReport(id: Guid.Parse("22222222-0005-0000-0000-000000000000"),
            usrn: 99999999999, impactStartUtc: BaseImpactDate);

    public static readonly FloodReport Road_DifferentAuthority =
        CreateReport(id: Guid.Parse("22222222-0006-0000-0000-000000000000"),
            usrn: 20023242411, impactStartUtc: BaseImpactDate, authorityId: LlfaId);

    public static readonly FloodReport Road_GroundwaterWindow =
        CreateReport(id: Guid.Parse("22222222-0007-0000-0000-000000000000"),
            usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-10), impactDurationHours: 1);

    public static readonly FloodReport Road_OngoingOnlyMatch =
        CreateReport(id: Guid.Parse("22222222-0008-0000-0000-000000000000"),
            usrn: 20023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(3));

    public static readonly FloodReport Road_OngoingCapExceeded =
        CreateReport(id: Guid.Parse("22222222-0009-0000-0000-000000000000"),
            usrn: 20023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(21));

    public static readonly FloodReport Road_OngoingHistorical =
        CreateReport(id: Guid.Parse("22222222-0010-0000-0000-000000000000"),
            usrn: 20023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(-100));

    // Location reports
    public static readonly FloodReport Location_Within25m =
        CreateReport(id: Guid.Parse("33333333-0001-0000-0000-000000000000"),
            easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate);

    public static readonly FloodReport Location_Within25m_Second =
        CreateReport(id: Guid.Parse("33333333-0002-0000-0000-000000000000"),
            easting: BaseEasting + 10, northing: BaseNorthing,
            impactStartUtc: BaseImpactDate.AddDays(-2), impactDurationHours: 48);

    public static readonly FloodReport Location_Outside25m =
        CreateReport(id: Guid.Parse("33333333-0003-0000-0000-000000000000"),
            easting: BaseEasting + 30, northing: BaseNorthing, impactStartUtc: BaseImpactDate);

    public static readonly FloodReport Location_TooEarly =
        CreateReport(id: Guid.Parse("33333333-0004-0000-0000-000000000000"),
            easting: BaseEasting + 20, northing: BaseNorthing,
            impactStartUtc: BaseImpactDate.AddDays(-30), impactDurationHours: 1);

    public static readonly FloodReport Location_TooLate =
        CreateReport(id: Guid.Parse("33333333-0005-0000-0000-000000000000"),
            easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate.AddDays(30));

    public static readonly FloodReport Location_DifferentAuthority =
        CreateReport(id: Guid.Parse("33333333-0006-0000-0000-000000000000"),
            easting: BaseEasting + 20, northing: BaseNorthing,
            impactStartUtc: BaseImpactDate, authorityId: LlfaId);

    public static readonly FloodReport Property_GroundwaterCapOnlyMatch =
        CreateReport(id: Guid.Parse("11111111-0011-0000-0000-000000000000"),
            uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(20));

    public static readonly FloodReport Road_GroundwaterCapOnlyMatch =
        CreateReport(id: Guid.Parse("22222222-0011-0000-0000-000000000000"),
            usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(20));

    // DB-side groundwater cap: groundwater cause, ongoing, AddDays(-22).
    // Effective end = -22 + 31 = +9 days >= incomingStart(-7) → MATCH.
    public static readonly FloodReport Property_OngoingGroundwaterDbCap =
        CreateReport(id: Guid.Parse("11111111-0012-0000-0000-000000000000"),
            uprn: 10023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(-22),
            causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]);

    public static readonly FloodReport Road_OngoingGroundwaterDbCap =
        CreateReport(id: Guid.Parse("22222222-0012-0000-0000-000000000000"),
            usrn: 20023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(-22),
            causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]);

    // DB-side flooding cap miss: river cause, ongoing, same start as above.
    // Effective end = -22 + 14 = -8 days < incomingStart(-7) → NO MATCH.
    public static readonly FloodReport Property_OngoingFloodingDbCapMiss =
        CreateReport(id: Guid.Parse("11111111-0013-0000-0000-000000000000"),
            uprn: 10023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(-22));

    public static readonly FloodReport Road_OngoingFloodingDbCapMiss =
        CreateReport(id: Guid.Parse("22222222-0013-0000-0000-000000000000"),
            usrn: 20023242411, isOnGoing: true, impactStartUtc: BaseImpactDate.AddDays(-22));

    // -------------------------------------------------------------------------
    // Single shared dataset — every test uses this collection, mirroring the
    // real FloodReports table which contains many unrelated records.
    // -------------------------------------------------------------------------
    public static readonly ICollection<FloodReport> AllReports =
    [
        Property_ExactDateMatch, Property_WithinWindow, Property_TooEarly, Property_TooLate,
        Property_DifferentUprn, Property_DifferentAuthority, Property_GroundwaterWindow,
        Property_OngoingOnlyMatch, Property_OngoingCapExceeded, Property_OngoingHistorical,
        Property_GroundwaterCapOnlyMatch, Property_OngoingGroundwaterDbCap, Property_OngoingFloodingDbCapMiss,
        Road_ExactDateMatch, Road_WithinWindow, Road_TooEarly, Road_TooLate,
        Road_DifferentUsrn, Road_DifferentAuthority, Road_GroundwaterWindow,
        Road_OngoingOnlyMatch, Road_OngoingCapExceeded, Road_OngoingHistorical,
        Road_GroundwaterCapOnlyMatch, Road_OngoingGroundwaterDbCap, Road_OngoingFloodingDbCapMiss,
        Location_Within25m, Location_Within25m_Second, Location_Outside25m,
        Location_TooEarly, Location_TooLate, Location_DifferentAuthority,
    ];

    // -------------------------------------------------------------------------
    // Theory data — reports array is gone; tests always use AllReports.
    // Match cases pass the ID that MUST be in the result.
    // No-match cases pass the ID that MUST NOT be in the result.
    // -------------------------------------------------------------------------

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> PropertyMatch_Cases()
    {
        var incomingRecord = CreateIncomingRecord(uprn: 10023242411);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 1", FloodReportType.Property, incomingRecord, Property_ExactDateMatch.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 2", FloodReportType.Property, incomingRecord, Property_WithinWindow.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> PropertyNoMatch_Cases()
    {
        var incomingRecord = CreateIncomingRecord(uprn: 10023242411);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 3", FloodReportType.Property, incomingRecord, Property_DifferentUprn.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 4", FloodReportType.Property, incomingRecord, Property_TooEarly.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 5", FloodReportType.Property, incomingRecord, Property_TooLate.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid, bool>> PropertyAuthority_Cases()
    {
        var incomingRecordEa = CreateIncomingRecord(uprn: 10023242411, authorityId: EnvironmentAgencyId);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid, bool>("Scenario 6", FloodReportType.Property, incomingRecordEa, Property_ExactDateMatch.Id, true);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid, bool>("Scenario 7", FloodReportType.Property, incomingRecordEa, Property_DifferentAuthority.Id, false);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid[]>> PropertyMultipleMatches_Cases()
    {
        var incomingRecord = CreateIncomingRecord(uprn: 10023242411);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid[]>
            ("Scenario 8", FloodReportType.Property, incomingRecord, [Property_ExactDateMatch.Id, Property_WithinWindow.Id]);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> PropertyOngoing_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>
            ("Scenario 9", FloodReportType.Property, CreateIncomingRecord(uprn: 10023242411, isOnGoing: true), Property_OngoingOnlyMatch.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> RoadMatch_Cases()
    {
        var incomingRecord = CreateIncomingRecord(usrn: 20023242411);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 10", FloodReportType.Road, incomingRecord, Road_ExactDateMatch.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 11", FloodReportType.Road, incomingRecord, Road_WithinWindow.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> RoadNoMatch_Cases()
    {
        var incomingRecord = CreateIncomingRecord(usrn: 20023242411);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 12", FloodReportType.Road, incomingRecord, Road_DifferentUsrn.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 13", FloodReportType.Road, incomingRecord, Road_TooEarly.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>("Scenario 14", FloodReportType.Road, incomingRecord, Road_TooLate.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid, bool>> RoadAuthority_Cases()
    {
        var incomingRecordEa = CreateIncomingRecord(usrn: 20023242411, authorityId: EnvironmentAgencyId);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid, bool>("Scenario 15", FloodReportType.Road, incomingRecordEa, Road_ExactDateMatch.Id, true);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid, bool>("Scenario 16", FloodReportType.Road, incomingRecordEa, Road_DifferentAuthority.Id, false);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid[]>> RoadMultipleMatches_Cases()
    {
        var incomingRecord = CreateIncomingRecord(usrn: 20023242411);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid[]>
            ("Scenario 17", FloodReportType.Road, incomingRecord, [Road_ExactDateMatch.Id, Road_WithinWindow.Id]);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> RoadOngoing_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>
            ("Scenario 18", FloodReportType.Road, CreateIncomingRecord(usrn: 20023242411, isOnGoing: true), Road_OngoingOnlyMatch.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid>> LocationMatch_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid>
            ("Scenario 19", FloodReportType.Location, CreateIncomingRecord(easting: BaseEasting, northing: BaseNorthing), 25, Location_Within25m.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid>> LocationNoMatch_Cases()
    {
        var incomingRecord = CreateIncomingRecord(easting: BaseEasting, northing: BaseNorthing);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid>("Scenario 20", FloodReportType.Location, incomingRecord, 25, Location_Outside25m.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid>("Scenario 21", FloodReportType.Location, incomingRecord, 25, Location_TooEarly.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid>("Scenario 22", FloodReportType.Location, incomingRecord, 25, Location_TooLate.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid, bool>> LocationAuthority_Cases()
    {
        var incomingRecordEa = CreateIncomingRecord(easting: BaseEasting, northing: BaseNorthing, authorityId: EnvironmentAgencyId);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid, bool>("Scenario 23", FloodReportType.Location, incomingRecordEa, 25, Location_Within25m.Id, true);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid, bool>("Scenario 24", FloodReportType.Location, incomingRecordEa, 25, Location_DifferentAuthority.Id, false);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid[]>> LocationMultipleMatches_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, double, Guid[]>(
            "Scenario 25", FloodReportType.Location, CreateIncomingRecord(easting: BaseEasting, northing: BaseNorthing), 25,
            [Location_Within25m.Id, Location_Within25m_Second.Id]);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> GroundwaterBuffer_ReturnsMatch_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 26", FloodReportType.Property, CreateIncomingRecord(uprn: 10023242411, causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]), Property_GroundwaterWindow.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 27", FloodReportType.Road, CreateIncomingRecord(usrn: 20023242411, causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]), Road_GroundwaterWindow.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> GroundwaterBuffer_ReturnsNoMatch_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 28", FloodReportType.Property, CreateIncomingRecord(uprn: 10023242411, causeIds: [PrimaryCauseIds.River]), Property_GroundwaterWindow.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 29", FloodReportType.Road, CreateIncomingRecord(usrn: 20023242411, causeIds: [PrimaryCauseIds.River]), Road_GroundwaterWindow.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> PropertyOngoingNoMatch_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 31", FloodReportType.Property, CreateIncomingRecord(uprn: 10023242411, isOnGoing: true), Property_OngoingCapExceeded.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 32", FloodReportType.Property, CreateIncomingRecord(uprn: 10023242411), Property_OngoingHistorical.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> RoadOngoingNoMatch_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 33", FloodReportType.Road, CreateIncomingRecord(usrn: 20023242411, isOnGoing: true), Road_OngoingCapExceeded.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 34", FloodReportType.Road, CreateIncomingRecord(usrn: 20023242411), Road_OngoingHistorical.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> GroundwaterOngoingCap_ReturnsMatch_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 35", FloodReportType.Property,
            CreateIncomingRecord(uprn: 10023242411, isOnGoing: true, causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]),
            Property_GroundwaterCapOnlyMatch.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 36", FloodReportType.Road,
            CreateIncomingRecord(usrn: 20023242411, isOnGoing: true, causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]),
            Road_GroundwaterCapOnlyMatch.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 39", FloodReportType.Property,
            CreateIncomingRecord(uprn: 10023242411),
            Property_OngoingGroundwaterDbCap.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 40", FloodReportType.Road,
            CreateIncomingRecord(usrn: 20023242411),
            Road_OngoingGroundwaterDbCap.Id);
    }

    public static IEnumerable<TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>> GroundwaterOngoingCap_ReturnsNoMatch_Cases()
    {
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 37", FloodReportType.Property,
            CreateIncomingRecord(uprn: 10023242411, isOnGoing: true, causeIds: [PrimaryCauseIds.River]),
            Property_GroundwaterCapOnlyMatch.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 38", FloodReportType.Road,
            CreateIncomingRecord(usrn: 20023242411, isOnGoing: true, causeIds: [PrimaryCauseIds.River]),
            Road_GroundwaterCapOnlyMatch.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 41", FloodReportType.Property,
            CreateIncomingRecord(uprn: 10023242411),
            Property_OngoingFloodingDbCapMiss.Id);
        yield return new TheoryDataRow<string, FloodReportType, EligibilityCheckRecord, Guid>(
            "Scenario 42", FloodReportType.Road,
            CreateIncomingRecord(usrn: 20023242411),
            Road_OngoingFloodingDbCapMiss.Id);
    }

    // -------------------------------------------------------------------------
    // Factories
    // -------------------------------------------------------------------------

    public static FloodReport CreateReport(
        Guid? id = null,
        FloodReportType recordType = FloodReportType.Property,
        long? uprn = null,
        long? usrn = null,
        double? easting = null,
        double? northing = null,
        DateTimeOffset? impactStartUtc = null,
        int impactDurationHours = 5,
        bool isOnGoing = false,
        ICollection<Guid>? causeIds = null,
        Guid? authorityId = null)
    {
        var reportId = id ?? Guid.NewGuid();
        var sourceRef = $"TEST-{reportId.ToString()[..8].ToUpperInvariant()}";
        var effectiveAuthorityId = authorityId ?? EnvironmentAgencyId;

        var causes = (causeIds ?? [PrimaryCauseIds.River])
            .Select(causeId => new FloodReportSourceFloodCause
            {
                FloodReportSourceRef = sourceRef,
                CauseId = causeId,
                Name = causeId.ToString(),
            }).ToList();

        ICollection<FloodReportSourceOrganisation> organisations =
        [
            new FloodReportSourceOrganisation
            {
                Id = Guid.NewGuid(),
                OrganisationId = Guid.NewGuid(),
                OrganisationName = "Test Organisation",
                FloodAuthorityId = effectiveAuthorityId,
                FloodAuthorityName = effectiveAuthorityId == EnvironmentAgencyId ? "Environment Agency" : "LLFA",
                FloodReportSourceRef = sourceRef,
            }
        ];

        var source = new FloodReportSource
        {
            FloodReportId = reportId,
            Reference = "TEST-REF",
            ViewUri = new Uri("https://example.com/test"),
            CreatedUtc = BaseImpactDate,
            EligibilityCheckId = Guid.NewGuid(),
            Uprn = uprn,
            Usrn = usrn,
            Geom = new Point(easting ?? BaseEasting, northing ?? BaseNorthing) { SRID = 27700 },
            ImpactStartUtc = impactStartUtc ?? BaseImpactDate,
            ImpactDurationHours = impactDurationHours,
            IsOnGoing = isOnGoing,
            IsUninhabitable = false,
            FloodCauses = causes,
            Organisations = organisations,
        };

        return new FloodReport
        {
            Id = reportId,
            Reference = $"TEST-{reportId.ToString()[..8].ToUpperInvariant()}",
            RecordStatus = RecordStatusIds.New,
            RecordType = recordType,
            Geom = new Point(easting ?? BaseEasting, northing ?? BaseNorthing) { SRID = 27700 },
            CreatedUtc = BaseImpactDate,
            SourceRecords = [source],
        };
    }

    public static EligibilityCheckRecord CreateIncomingRecord(
        long? uprn = null,
        long? usrn = null,
        double? easting = null,
        double? northing = null,
        DateTimeOffset? impactStartUtc = null,
        int impactDurationHours = 24,
        bool isOnGoing = false,
        ICollection<Guid>? causeIds = null,
        Guid? authorityId = null)
    {
        var effectiveAuthorityId = authorityId ?? EnvironmentAgencyId;
        var causes = (causeIds ?? [PrimaryCauseIds.River])
            .Select(id => new EligibilityCheckFloodSource(id, id.ToString()))
            .ToList();

        IReadOnlyCollection<EligibilityCheckOrganisation> organisations =
        [
            new EligibilityCheckOrganisation(
                Guid.NewGuid(),
                "Test Organisation",
                effectiveAuthorityId,
                effectiveAuthorityId == EnvironmentAgencyId ? "Environment Agency" : "LLFA")
        ];

        return new EligibilityCheckRecord(
            Guid.NewGuid(), uprn, usrn,
            easting ?? BaseEasting, northing ?? BaseNorthing,
            impactStartUtc ?? BaseImpactDate,
            impactDurationHours, isOnGoing, false, 0, "Test location",
            organisations, causes);
    }
}