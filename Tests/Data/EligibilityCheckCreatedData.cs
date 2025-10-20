using FloodOnlineReportingTool.Contracts;
using MassTransit;

namespace Tests.Data;

public static class EligibilityCheckCreatedData
{
    public static readonly EligibilityCheckCreated Ecc1 = new(

    );
    public static readonly EligibilityCheckCreated Ecc2 = new(

    );
    public static readonly EligibilityCheckCreated Ecc3 = new(

    );
    public static readonly EligibilityCheckCreated Ecc4 = new(

    );

    public static IEnumerable<object[]> FloodProperty_4 => [
        [ Ecc1, "Some test location 1" ],
        [ Ecc2, "Some test location 2" ],
        [ Ecc3, "Some test location 3" ],
        [ Ecc4, "Some test location 1" ],
    ];
}
