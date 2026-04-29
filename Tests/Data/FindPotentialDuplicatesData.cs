using FloodOnlineReportingTool.Contracts;
using FloodOnlineReportingTool.Contracts.Shared;
using FloodOnlineReportingTool.ReportStatus.Database.Models;
using NetTopologySuite.Geometries;

namespace Tests.Data;

public static class FindPotentialDuplicatesData
{
    private static readonly DateTimeOffset BaseImpactDate = new(2024, 12, 10, 10, 0, 0, TimeSpan.Zero);
    private static readonly double BaseEasting = 368991.12;
    private static readonly double BaseNorthing = 90881.94;

    // Fixed authority IDs — no FloodAuthorityIds constants exist in the contracts package,
    // so we define stable GUIDs here that represent EA and LLFA for testing purposes
    internal static readonly Guid EnvironmentAgencyId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
    internal static readonly Guid LlfaId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002");

    /// <summary>
    /// Creates a <see cref="FloodReport"/> with a single source record for duplicate detection testing.
    /// Authority defaults to <see cref="EnvironmentAgencyId"/> unless overridden.
    /// </summary>
    public static FloodReport CreateReport(
        Guid? id = null,
        FloodReportType recordType = FloodReportType.Property,
        long? uprn = null,
        long? usrn = null,
        double? easting = null,
        double? northing = null,
        DateTimeOffset? impactStartUtc = null,
        int impactDurationHours = 5,
        ICollection<Guid>? causeIds = null,
        Guid? authorityId = null)
    {
        var reportId = id ?? Guid.NewGuid();
        var sourceId = Guid.NewGuid();
        var effectiveAuthorityId = authorityId ?? EnvironmentAgencyId;

        var causes = (causeIds ?? [PrimaryCauseIds.River])
            .Select(causeId => new FloodReportSourceFloodCause
            {
                FloodReportSourceId = sourceId,
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
                    FloodReportSourceId = sourceId,
                }
        ];

        var source = new FloodReportSource
        {
            Id = sourceId,
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
            IsOnGoing = false,
            IsUninhabitable = false,
            FloodCauses = causes,
            Organisations = organisations,
        };

        return new FloodReport
        {
            Id = reportId,
            RecordStatus = RecordStatusIds.New,
            RecordType = recordType,
            CreatedUtc = BaseImpactDate,
            SourceRecords = [source],
        };
    }

    /// <summary>
    /// Creates an <see cref="EligibilityCheckRecord"/> for the incoming event being checked for duplicates.
    /// Authority defaults to <see cref="EnvironmentAgencyId"/> unless overridden.
    /// </summary>
    public static EligibilityCheckRecord CreateEcr(
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

        // EligibilityCheckOrganisation(Guid Id, string Name, Guid FloodAuthorityId, string FloodAuthorityName)
        IReadOnlyCollection<EligibilityCheckOrganisation> organisations =
        [
            new EligibilityCheckOrganisation(
                    Guid.NewGuid(),
                    "Test Organisation",
                    effectiveAuthorityId,
                    effectiveAuthorityId == EnvironmentAgencyId ? "Environment Agency" : "LLFA")
        ];

        return new EligibilityCheckRecord(
            Guid.NewGuid(),
            uprn,
            usrn,
            easting ?? BaseEasting,
            northing ?? BaseNorthing,
            impactStartUtc ?? BaseImpactDate,
            impactDurationHours,
            isOnGoing,
            false,
            0,
            "Test location",
            organisations,
            causes
        );
    }

    // --- Match cases ---

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> PropertyMatch_Cases() =>
        new()
        {
            // Exact date match — incoming and existing share the same start date
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate)], FloodReportType.Property, CreateEcr(uprn: 10023242411), 1 },
            // Within 7-day window — existing event ended 3 days before incoming, inside buffer
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1)], FloodReportType.Property, CreateEcr(uprn: 10023242411), 1 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> PropertyNoMatch_Cases() =>
        new()
        {
            // Different UPRN
            { [CreateReport(uprn: 99999999999, impactStartUtc: BaseImpactDate)], FloodReportType.Property, CreateEcr(uprn: 10023242411), 0 },
            // Too early — existing ended 23 days ago, outside the 7-day buffer window
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-30), impactDurationHours: 1)], FloodReportType.Property, CreateEcr(uprn: 10023242411), 0 },
            // Too late — existing starts 30 days after the incoming event ends
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(30))], FloodReportType.Property, CreateEcr(uprn: 10023242411), 0 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> PropertyAuthorityMatch_Cases() =>
        new()
        {
            // Same UPRN, same dates, same authority → match
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate, authorityId: EnvironmentAgencyId)], FloodReportType.Property, CreateEcr(uprn: 10023242411, authorityId: EnvironmentAgencyId), 1 },
            // Same UPRN, same dates, different authority → no match
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate, authorityId: LlfaId)], FloodReportType.Property, CreateEcr(uprn: 10023242411, authorityId: EnvironmentAgencyId), 0 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> PropertyMultipleMatches_Cases() =>
        new()
        {
            // 2 reports, both matching UPRN, authority and date window → 2
            {
                [
                    CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate),
                    CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1)
                ],
                FloodReportType.Property, CreateEcr(uprn: 10023242411), 2
            },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> PropertyOngoing_Cases() =>
        new()
        {
            // Ongoing incoming — incomingEnd becomes UtcNow so past events within window match
            {
                [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1)],
                FloodReportType.Property, CreateEcr(uprn: 10023242411, isOnGoing: true), 1
            },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> RoadMatch_Cases() =>
        new()
        {
            // Exact date match, same USRN, same authority
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate)], FloodReportType.Road, CreateEcr(usrn: 20023242411), 1 },
            // Within 7-day window
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1)], FloodReportType.Road, CreateEcr(usrn: 20023242411), 1 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> RoadNoMatch_Cases() =>
        new()
        {
            // Different USRN
            { [CreateReport(usrn: 99999999999, impactStartUtc: BaseImpactDate)], FloodReportType.Road, CreateEcr(usrn: 20023242411), 0 },
            // Too early
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-30), impactDurationHours: 1)], FloodReportType.Road, CreateEcr(usrn: 20023242411), 0 },
            // Too late
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(30))], FloodReportType.Road, CreateEcr(usrn: 20023242411), 0 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> RoadAuthorityMatch_Cases() =>
        new()
        {
            // Same USRN, same dates, same authority → match
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate, authorityId: EnvironmentAgencyId)], FloodReportType.Road, CreateEcr(usrn: 20023242411, authorityId: EnvironmentAgencyId), 1 },
            // Same USRN, same dates, different authority → no match
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate, authorityId: LlfaId)], FloodReportType.Road, CreateEcr(usrn: 20023242411, authorityId: EnvironmentAgencyId), 0 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> RoadMultipleMatches_Cases() =>
        new()
        {
            {
                [
                    CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate),
                    CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1)
                ],
                FloodReportType.Road, CreateEcr(usrn: 20023242411), 2
            },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> RoadOngoing_Cases() =>
        new()
        {
            {
                [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-3), impactDurationHours: 1)],
                FloodReportType.Road, CreateEcr(usrn: 20023242411, isOnGoing: true), 1
            },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, double, int> LocationMatch_Cases() =>
        new()
        {
            // 20m away (within 25m buffer), same authority, exact date
            { [CreateReport(easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate)], FloodReportType.Location, CreateEcr(easting: BaseEasting, northing: BaseNorthing), 25, 1 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, double, int> LocationNoMatch_Cases() =>
        new()
        {
            // Outside 25m buffer (30m away)
            { [CreateReport(easting: BaseEasting + 30, northing: BaseNorthing, impactStartUtc: BaseImpactDate)], FloodReportType.Location, CreateEcr(easting: BaseEasting, northing: BaseNorthing), 25, 0 },
            // Within buffer but date too early
            { [CreateReport(easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate.AddDays(-30), impactDurationHours: 1)], FloodReportType.Location, CreateEcr(easting: BaseEasting, northing: BaseNorthing), 25, 0 },
            // Within buffer but date too late
            { [CreateReport(easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate.AddDays(30))], FloodReportType.Location, CreateEcr(easting: BaseEasting, northing: BaseNorthing), 25, 0 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, double, int> LocationAuthorityMatch_Cases() =>
        new()
        {
            // Within buffer, same authority → match
            { [CreateReport(easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate, authorityId: EnvironmentAgencyId)], FloodReportType.Location, CreateEcr(easting: BaseEasting, northing: BaseNorthing, authorityId: EnvironmentAgencyId), 25, 1 },
            // Within buffer, different authority → no match
            { [CreateReport(easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate, authorityId: LlfaId)], FloodReportType.Location, CreateEcr(easting: BaseEasting, northing: BaseNorthing, authorityId: EnvironmentAgencyId), 25, 0 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, double, int> LocationMultipleMatches_Cases() =>
        new()
        {
            {
                [
                    CreateReport(easting: BaseEasting + 10, northing: BaseNorthing, impactStartUtc: BaseImpactDate),
                    CreateReport(easting: BaseEasting + 20, northing: BaseNorthing, impactStartUtc: BaseImpactDate.AddDays(-2), impactDurationHours: 48)
                ],
                FloodReportType.Location, CreateEcr(easting: BaseEasting, northing: BaseNorthing), 25, 2
            },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> GroundwaterBuffer_ReturnsMatch_Cases() =>
        new()
        {
            // Property: groundwater incoming → 14-day buffer → event 10 days ago matches
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-10), impactDurationHours: 1)], FloodReportType.Property, CreateEcr(uprn: 10023242411, causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]), 1 },
            // Road: groundwater incoming → 14-day buffer → event 10 days ago matches
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-10), impactDurationHours: 1)], FloodReportType.Road, CreateEcr(usrn: 20023242411, causeIds: [PrimaryCauseIds.WaterRisingOutOfTheGround]), 1 },
        };

    public static TheoryData<FloodReport[], FloodReportType, EligibilityCheckRecord, int> GroundwaterBuffer_ReturnsNoMatch_Cases() =>
        new()
        {
            // Property: river incoming → 7-day buffer → event 10 days ago is outside window
            { [CreateReport(uprn: 10023242411, impactStartUtc: BaseImpactDate.AddDays(-10), impactDurationHours: 1)], FloodReportType.Property, CreateEcr(uprn: 10023242411, causeIds: [PrimaryCauseIds.River]), 0 },
            // Road: river incoming → 7-day buffer → same event outside window
            { [CreateReport(usrn: 20023242411, impactStartUtc: BaseImpactDate.AddDays(-10), impactDurationHours: 1)], FloodReportType.Road, CreateEcr(usrn: 20023242411, causeIds: [PrimaryCauseIds.River]), 0 },
        };
}