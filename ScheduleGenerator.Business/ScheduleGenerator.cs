using System.Net.Http.Json;
using System.Text.Json.Nodes;
using ScheduleGenerator.Entities;
using Microsoft.Extensions.Options;
using ScheduleGenerator.Business.Model;

namespace ScheduleGenerator.Business;

public class ScheduleGenerator : IScheduleGenerator
{
    private readonly HttpClient httpClient;
    private readonly IOptions<ScheduleGeneratorSettings> settings;

    public ScheduleGenerator(HttpClient httpClient, IOptions<ScheduleGeneratorSettings> settings)
    {
        this.httpClient = httpClient;
        this.settings = settings;
    }

    public async Task<Schedule> Generate(IEnumerable<Tray> trays)
    {
        var tray = trays.First();

        var recipeResponse = await httpClient.GetAsync(settings.Value.RecipeUri);
        var recipes = await recipeResponse.Content.ReadFromJsonAsync<Recipes>();

        var recipe = recipes.AllRecipes.First(x => x.Name == tray.RecipeName);

        var lightingPhases = recipe.LightingPhases;
        var lightingSchedule = CreateLightingSchedule(tray.StartDate, lightingPhases);

        var wateringPhases = recipe.WateringPhases;
        var wateringSchedule = CreateWateringSchedule(tray.StartDate, wateringPhases);

        Schedule.Tray scheduleTray = new()
        {
            Name = recipe.Name,
            LightingCommands = lightingSchedule,
            WateringCommands = wateringSchedule
        };

        Schedule schedule = new()
        {
            Trays = new List<Schedule.Tray>()
            {
                scheduleTray
            }
        };

        return schedule;
    }

    public IEnumerable<Schedule.Tray.LightingCommand> CreateLightingSchedule(DateTime startDate, IEnumerable<LightingPhase> lightingPhases)
    {
        List<Schedule.Tray.LightingCommand> lightingCommands = new();
        var lightingStartDate = startDate;
        foreach (var phase in lightingPhases.OrderBy(x => x.Order))
        {
            for (int i = 0; i < phase.Repetitions; i++)
            {
                foreach (var operation in phase.Operations)
                {
                    var offsetHours = operation.OffsetHours;
                    var offsetMinutes = operation.OffsetMinutes;

                    TimeSpan commandOffset = new(offsetHours.Value, offsetMinutes.Value, 0);
                    var commandStart = lightingStartDate + commandOffset;

                    Schedule.Tray.LightingCommand command = new()
                    {
                        At = commandStart,
                        LightIntensity = operation.LightIntensity.Value
                    };

                    lightingCommands.Add(command);
                }

                lightingStartDate += new TimeSpan(phase.Hours.Value, phase.Minutes.Value, 0);
            }
        }

        return lightingCommands;
    }

    public IEnumerable<Schedule.Tray.WateringCommand> CreateWateringSchedule(DateTime startDate, IEnumerable<WateringPhase> wateringPhases)
    {
        List<Schedule.Tray.WateringCommand> wateringCommands = new();
        var wateringStartDate = startDate;
        foreach (var phase in wateringPhases.OrderBy(x => x.Order))
        {
            for (int i = 0; i < phase.Repetitions; i++)
            {
                Schedule.Tray.WateringCommand command = new()
                {
                    At = wateringStartDate,
                    Amount = phase.Amount.Value,
                    Duration = new TimeSpan(phase.Hours.Value, phase.Minutes.Value, 0)
                };

                wateringCommands.Add(command);
                wateringStartDate += new TimeSpan(phase.Hours.Value, phase.Minutes.Value, 0);
            }
        }

        return wateringCommands;
    }
}
