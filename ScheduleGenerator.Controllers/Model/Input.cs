using System.Text.Json.Serialization;
using ScheduleGenerator.Entities;

namespace ScheduleGeneratorApi.Model;

public sealed record Input
{
    [JsonPropertyName("Input")]
    public IEnumerable<Tray> Trays { get; init; } = Enumerable.Empty<Tray>();
}
