namespace FloodOnlineReportingTool.ReportStatus.Endpoints;

/// <summary>Controls which reference field is searched when looking up a flood report by reference.</summary>
public enum FloodReferenceSearchType
{
    /// <summary>Search against the flood report's own generated reference (e.g. "A1B2C3D4").</summary>
    Report,

    /// <summary>Search against the originating source system's reference.</summary>
    Source,
}