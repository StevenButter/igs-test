using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ScheduleGenerator.Entities;

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
        List<Tray> trays = new()
        {
            new Tray{ RecipeName = "Basil", StartDate = DateTime.Now, TrayNumber = 1 }
        };

        var response = await httpClient.PostAsync("/schedule", JsonContent.Create(trays));
        var schedule = await response.Content.ReadFromJsonAsync<Schedule>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(schedule);
    }
}