using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using ScheduleGenerator.Entities;

namespace ScheduleGenerator.Business;

public class ScheduleGenerator
{
    const string RecipeUri = "http://localhost:8080/recipe";
    private readonly HttpClient httpClient;

    public ScheduleGenerator(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Schedule> Generate(IEnumerable<Tray> trays)
    {
        var tray = trays.First();

        var recipeResponse = await httpClient.GetAsync(RecipeUri);
        JsonNode? rsp = await recipeResponse.Content.ReadFromJsonAsync<JsonNode>();

        var recipes = (IEnumerable<JsonNode>?)rsp["recipes"];//.recipes;

        var recipe = recipes.First(x => (string)x["name"] == tray.RecipeName);

        var lightingPhases = (IEnumerable<JsonNode>?)recipe["lightingPhases"];

        List<Schedule.Tray.LightingCommand> lightingCommands = new();
        var lightingStartDate = tray.StartDate;
        foreach (var phase in lightingPhases.OrderBy(x => (int)x["order"]))
        {
            for (int i = 0; i < (int)phase["repetitions"]; i++)
            {
                foreach (var operation in (IEnumerable<JsonNode>)phase["operations"])
                {
                    var offsetHours = (int)operation["offsetHours"];
                    var offsetMinutes = (int)operation["offsetMinutes"];

                    TimeSpan commandOffset = new(offsetHours, offsetMinutes, 0);
                    var commandStart = lightingStartDate + commandOffset;

                    Schedule.Tray.LightingCommand command = new()
                    {
                        At = commandStart,
                        LightIntensity = (int)operation["lightIntensity"]
                    };

                    lightingCommands.Add(command);
                }

                lightingStartDate += new TimeSpan((int)phase["hours"], (int)phase["minutes"], 0);
            }
        }

        var wateringPhases = (IEnumerable<JsonNode>)recipe["wateringPhases"];
        List<Schedule.Tray.WateringCommand> wateringCommands = new();
        var wateringStartDate = tray.StartDate;
        foreach (var phase in wateringPhases.OrderBy(x => (int)x["order"]))
        {
            for (int i = 0; i < (int)phase["repetitions"]; i++)
            {
                Schedule.Tray.WateringCommand command = new()
                {
                    At = wateringStartDate,
                    Amount = (int)phase["amount"],
                    Duration = new TimeSpan((int)phase["hours"], (int)phase["minutes"], 0)
                };

                wateringCommands.Add(command);
                wateringStartDate += new TimeSpan((int)phase["hours"], (int)phase["minutes"], 0);
            }
        }

        Schedule.Tray scheduleTray = new()
        {
            Name = (string)recipe["name"],
            LightingCommands = lightingCommands,
            WateringCommands = wateringCommands

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
}
