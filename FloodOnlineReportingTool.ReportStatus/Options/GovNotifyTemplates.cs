namespace FloodOnlineReportingTool.ReportStatus.Options;

public class GovNotifyTemplates
{
    public required string Vulnerable { get; init; }

    /*
    /// <summary>
    ///     <para>GovNotify template Id under Area > Reporter Updates. Name: Area Read</para>
    ///     <para>Was named AreaRead in SWIM</para>
    /// </summary>
    public required string CommentRead { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Area > Submission. Name: Area New</para>
    ///     <para>Was named AreaNew in SWIM</para>
    /// </summary>
    public required string FloodAreaRecordCreated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Area > Subscription. Name: Area RAG Update</para>
    ///     <para>Was named AreaRAGUpdate in SWIM</para>
    /// </summary>
    public required string FloodAreaStatusChanged { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Area > Subscription. Name: Area Subscriber Action Notification</para>
    ///     <para>Was named AreaSubAction in SWIM</para>
    /// </summary>
    public required string CommentReadReceipt { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Area > Subscription. Name: Area Subscriber</para>
    ///     <para>Was named AreaSub in SWIM</para>
    /// </summary>
    public required string NewArea { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Area > Subscription. Name: Area Subscriber Update</para>
    ///     <para>Was named AreaSubUpdate in SWIM</para>
    /// </summary>
    public required string AreaUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Contact. Name: Contacts Access Request</para>
    ///     <para>Was named ContactAccess in SWIM</para>
    /// </summary>
    public required string ContactAccessRequest { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Contact. Name: Contacts New</para>
    ///     <para>Was named ContactNew in SWIM</para>
    /// </summary>
    public required string NewContact { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Activity Deleted</para>
    ///     <para>Was named GroupActivityDelete in SWIM</para>
    /// </summary>
    public required string FloodGroupActivityDeleted { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Activity Request</para>
    ///     <para>Was Named GroupActivityNew in SWIM</para>
    /// </summary>
    public required string FloodGroupActivityRequest { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Activity Request Created</para>
    ///     <para>Was named GroupActivityMyNew in SWIM</para>
    /// </summary>
    public required string NewFloodGroupActivity { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Activity Update</para>
    ///     <para>Was named GroupActivityUpdate in SWIM</para>
    /// </summary>
    public required string FloodGroupActivityUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Cover Status</para>
    ///     <para>Was named GroupCoverStatus in SWIM</para>
    /// </summary>
    public required string FloodGroupCoverRequest { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Cover Update</para>
    ///     <para>Was named GroupCoverUpdate in SWIM</para>
    /// </summary>
    public required string FloodGroupCoverUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Delete Group</para>
    ///     <para>Was named ContactDelete in SWIM</para>
    /// </summary>
    public required string FloodGroupCoverDeleted { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: Group Update</para>
    ///     <para>Was named GroupUpdate in SWIM</para>
    /// </summary>
    public required string FloodGroupUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Group > Flood Group. Name: New Group</para>
    ///     <para>Was named GroupNew in SWIM</para>
    /// </summary>
    public required string NewFloodGroup { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Reporter Updates. Name: Point - Action Taken</para>
    ///     <para>Was named PointReporterAction in SWIM</para>
    /// </summary>
    public required string PropertyLevelActionsTaken { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Reporter Updates. Name: Point - Read Request</para>
    ///     <para>Was named PointReporterRead in SWIM</para>
    /// </summary>
    public required string PropertyLevelCommentReviewed { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Reporter Updates. Name: Point Reset Access Code</para>
    ///     <para>Was named PointReporterAccess in SWIM</para>
    /// </summary>
    public required string NewPropertyLevelAccessCode { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Reporter Updates. Name: Point Status Change</para>
    ///     <para>Was named PointReporterStatus in SWIM</para>
    /// </summary>
    public required string PropertyLevelStatusUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Reporter Updates. Name: Point Update</para>
    ///     <para>Was Named PointReporterUpdate in SWIM</para>
    /// </summary>
    public required string PropertyLevelUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Submission. Name: Point - DPA Signed</para>
    ///     <para>Was named PointDPASigned in SWIM</para>
    /// </summary>
    public required string PropertyLevelDpaSigned { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Submission. Name: Point Submission</para>
    ///     <para>Was named PointNew in SWIM</para>
    /// </summary>
    public required string NewPropertyLevel { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Submission. Name: Point Submission - DPA</para>
    ///     <para>Was named PointNewDPA in SWIM</para>
    /// </summary>
    public required string NewPropertyLevelDpa { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Subscription. Name: Point - DPA Update.</para>
    ///     <para>Was named PointSubDPA in SWIM</para>
    /// </summary>
    public required string PropertyLevelSubscriberDpaUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Subscription. Name: Point - RAG Update</para>
    ///     <para>Was named PointSubRAG in SWIM</para>
    /// </summary>
    public required string PropertyLevelSubscriberStatusUpdated { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Subscription. Name: Point - Subscriber Action</para>
    ///     <para>Was named PointSubAction in SWIM</para>
    /// </summary>
    public required string PropertyLevelSubscriberActionsTaken { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Subscription. Name: Point - Subscriber Read</para>
    ///     <para>Was named PointSubRead in SWIM</para>
    /// </summary>
    public required string PropertyLevelSubscriberCommentReadReceipt { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Subscription. Name: Point - Subscribers</para>
    ///     <para>Was named PointSub in SWIM</para>
    /// </summary>
    public required string NewPropertyLevelSubscriber { get; init; }

    /// <summary>
    ///     <para>GovNotify template Id under Point > Subscription. Name: Point - Subscriber Update</para>
    ///     <para>Was named PointSubUpdate in SWIM</para>
    /// </summary>
    public required string PropertyLevelSubscriberUpdated { get; init; }
    */
}
