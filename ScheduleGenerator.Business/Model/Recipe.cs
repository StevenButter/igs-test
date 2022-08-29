using System.Text.Json.Serialization;

namespace ScheduleGenerator.Business.Model;

public sealed record Recipes
{
    [JsonPropertyName("Recipes")]
    public IEnumerable<Recipe> AllRecipes { get; init; } = Enumerable.Empty<Recipe>();
}

public sealed record Recipe
{
    public string Name { get; init; } = "";
    public IEnumerable<LightingPhase> LightingPhases { get; init; } = Enumerable.Empty<LightingPhase>();
    public IEnumerable<WateringPhase> WateringPhases { get; init; } = Enumerable.Empty<WateringPhase>();
}

public abstract record Phase
{
    public string Name { get; init; } = "";
    public int? Order { get; init; }
    public int? Hours { get; init; }
    public int? Minutes { get; init; }
    public int? Repetitions { get; init; }
}

public sealed record LightingPhase : Phase
{
    public IEnumerable<Operation> Operations { get; init; } = Enumerable.Empty<Operation>();
}

public sealed record WateringPhase : Phase
{
    public int? Amount { get; init; }
}

public sealed record Operation
{
    public int? OffsetHours { get; init; }
    public int? OffsetMinutes { get; init; }
    public int? LightIntensity { get; init; }
}
