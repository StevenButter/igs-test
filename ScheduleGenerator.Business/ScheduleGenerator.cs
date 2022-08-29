using System.Net.Http.Json;
using ScheduleGenerator.Entities;
using Microsoft.Extensions.Options;
using ScheduleGenerator.Business.Model;
using ScheduleGenerator.Business.Creation;

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
        var recipes = await GetRecipes().ConfigureAwait(false);

        var traySchedule = trays.Select(tray =>
        {
            var recipe = recipes.First(x => x.Name == tray.RecipeName);
            return new Schedule.Tray
            {
                Name = recipe.Name,
                TrayNumber = tray.TrayNumber,
                LightingCommands = CreateLightingSchedule(tray.StartDate, recipe.LightingPhases),
                WateringCommands = CreateWateringSchedule(tray.StartDate, recipe.WateringPhases)
            };
        });

        return new Schedule { Trays = traySchedule };
    }

    private async Task<IEnumerable<Recipe>> GetRecipes()
    {
        var recipeResponse = await httpClient.GetAsync(settings.Value.RecipeUri);
        var recipes = await recipeResponse.Content.ReadFromJsonAsync<Recipes>();

        return recipes.AllRecipes;
    }

    private IEnumerable<Schedule.Tray.LightingCommand> CreateLightingSchedule(DateTime startDate, IEnumerable<LightingPhase> phases)
    {
        LightingScheduleCreator scheduleCreator = new();
        return scheduleCreator.Create(startDate, phases);
    }

    private IEnumerable<Schedule.Tray.WateringCommand> CreateWateringSchedule(DateTime startDate, IEnumerable<WateringPhase> phases)
    {
        WateringScheduleCreator scheduleCreator = new();
        return scheduleCreator.Create(startDate, phases);
    }
}
