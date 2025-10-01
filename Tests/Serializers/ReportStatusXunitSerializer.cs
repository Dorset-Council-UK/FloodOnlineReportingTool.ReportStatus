using FloodOnlineReportingTool.ReportStatus.Database.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Tests.Serializers;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(
    typeof(ReportStatusXunitSerializer),
    typeof(FloodLocation),
    typeof(FloodProperty),
    typeof(FloodReport),
    typeof(FloodReportSource),
    typeof(FloodRoad)
)]

namespace Tests.Serializers;

/// <summary>
///     <para>Provides serialization and deserialization support for specific flood-related types in the context of xUnit testing.</para>
///     <para><see href="https://xunit.net/docs/getting-started/v3/custom-serialization#serialization-support-in-v3">Serialization support in v3 - Custom Theory Data Serialization</see></para>
/// </summary>
/// <remarks>
/// This serializer supports the following types: <see cref="FloodLocation"/>, <see cref="FloodProperty"/>, <see cref="FloodReport"/>, <see cref="FloodReportSource"/>, and <see cref="FloodRoad"/>.
/// Types outside this list are not supported.
/// </remarks>
internal class ReportStatusXunitSerializer : IXunitSerializer
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public ReportStatusXunitSerializer()
    {
    }

    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        if (type is null)
        {
            failureReason = "Type is null";
            return false;
        }

        if (value is null)
        {
            failureReason = "Value is null";
            return false;
        }

        if (value is FloodLocation or FloodProperty or FloodReport or FloodReportSource or FloodRoad)
        {
            failureReason = null;
            return true;
        }

        failureReason = $"Type '{type.FullName}' is not supported by {nameof(ReportStatusXunitSerializer)}";
        return false;
    }

    public string Serialize(object value)
    {
        return JsonSerializer.Serialize(value, value.GetType(), _jsonSerializerOptions);
    }

    public object Deserialize(Type type, string serializedValue)
    {
        return JsonSerializer.Deserialize(serializedValue, type, _jsonSerializerOptions)
            ?? throw new InvalidOperationException($"Deserialization of type '{type.FullName}' returned null");
    }
}
