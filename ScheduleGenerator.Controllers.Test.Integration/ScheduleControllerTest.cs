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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("2021-13-08T17:33:00.0000000Z")]
    public async Task Post_InvalidStartDate_ReturnsBadRequest(string startDate)
    {
        var input = new
        {
            Input = new List<object>
            {
                new
                {
                    RecipeName = "Basil",
                    StartDate = startDate,
                    TrayNumber = 1
                }
            }
        };

        var response = await httpClient.PostAsync("/schedule", JsonContent.Create(input));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Post_InvalidRecipeName_ReturnsBadRequest(string recipeName)
    {
        var input = new
        {
            Input = new List<object>
            {
                new
                {
                    RecipeName = recipeName,
                    StartDate = DateTime.Now,
                    TrayNumber = 1
                }
            }
        };

        var response = await httpClient.PostAsync("/schedule", JsonContent.Create(input));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    // Missing TrayNumber
    [InlineData(@"{ ""input"": [{""RecipeName"":""Basil"", ""StartDate"":""2022-01-24T12:30:00.0000000Z""}]} ")]
    // Missing StartDate
    [InlineData(@"{ ""input"": [{""RecipeName"":""Basil"", ""TrayNumber"":1}]} ")]
    // Missing RecipeName
    [InlineData(@"{ ""input"": [{""StartDate"":""2022-01-24T12:30:00.0000000Z"", ""TrayNumber"":1}]} ")]
    public async Task Post_MissingInputs_ReturnsBadRequest(string input)
    {
        var response = await httpClient.PostAsync("/schedule", new StringContent(input, default, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}