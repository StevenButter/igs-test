using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ScheduleGenerator.Entities;
using ScheduleGeneratorApi.Model;

namespace ScheduleGenerator.Controllers.Test.Integration;

public class ScheduleControllerTest
{
    private readonly HttpClient httpClient;

    public ScheduleControllerTest()
    {
        var application = new WebApplicationFactory<Program>();
        httpClient = application.CreateClient();
    }

    [Fact]
    public async Task Post_ValidInput_ReturnsOk()
    {
        Input input = new()
        {
            Trays = new List<Tray>()
            {
                new Tray{ RecipeName = "Basil", StartDate = DateTime.Now, TrayNumber = 1 }
            }
        };

        var response = await httpClient.PostAsync("/schedule", JsonContent.Create(input));
        var schedule = await response.Content.ReadFromJsonAsync<Schedule>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(schedule);
    }
}