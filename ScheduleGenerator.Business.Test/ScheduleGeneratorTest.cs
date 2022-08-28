using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using ScheduleGenerator.Entities;
using FluentAssertions;

namespace ScheduleGenerator.Business.Test;

public class ScheduleGeneratorTest
{
    private readonly Mock<HttpMessageHandler> httpMessageHandler = new Mock<HttpMessageHandler>();
    private readonly HttpClient httpClient;
    private readonly ScheduleGenerator scheduleGenerator;

    public ScheduleGeneratorTest()
    {
        httpClient = new HttpClient(httpMessageHandler.Object);
        scheduleGenerator = new ScheduleGenerator(httpClient);
    }

    [Fact]
    public async Task Generate_ValidTrays_ReturnsValidSchedule()
    {
        var trays = CreateValidTrays();
        var responseContent = CreateValidRecipeResponse();
        SetupHttpGetResponse(responseContent);

        var schedule = await scheduleGenerator.Generate(trays);

        var expectedSchedule = CreateExpectedSchedule();

        schedule.Should().BeEquivalentTo(expectedSchedule);
    }

    private IEnumerable<Tray> CreateValidTrays()
    {
        return new List<Tray>
        {
            new Tray
            {
                RecipeName = "Basil",
                TrayNumber = 1,
                StartDate = DateTime.Parse("2022-01-24T12:30:00.0000000Z")
            }
        };
    }

    private HttpContent CreateValidRecipeResponse()
    {
        var root = new
        {
            recipes = new List<object>
            {
                new
                {
                    name = "Basil",
                    lightingPhases = new List<object>
                    {
                        new
                        {
                            operations = new List<object>
                            {
                                new
                                {
                                    offsetHours = 0,
                                    offsetMinutes = 0,
                                    lightIntensity = 1
                                },
                                new
                                {
                                    offsetHours = 6,
                                    offsetMinutes = 0,
                                    lightIntensity = 2
                                }
                            },
                            name = "LightingPhase 2",
                            order = 1,
                            hours = 8,
                            minutes = 0,
                            repetitions = 2

                        },
                        new
                        {
                            operations = new List<object>
                            {
                                new
                                {
                                    offsetHours = 0,
                                    offsetMinutes = 0,
                                    lightIntensity = 1
                                },
                                new
                                {
                                    offsetHours = 6,
                                    offsetMinutes = 0,
                                    lightIntensity = 2
                                },
                                new
                                {
                                    offsetHours = 12,
                                    offsetMinutes = 0,
                                    lightIntensity = 3
                                },
                                new
                                {
                                    offsetHours = 16,
                                    offsetMinutes = 0,
                                    lightIntensity = 0
                                }
                            },
                            name = "LightingPhase 1",
                            order = 0,
                            hours = 24,
                            minutes = 0,
                            repetitions = 2
                        }
                    },
                    wateringPhases = new List<object>
                    {
                        new
                        {
                            amount = 50,
                            name = "Watering Phase 1",
                            order = 1,
                            hours = 12,
                            minutes = 0,
                            repetitions = 2
                        },
                        new
                        {
                            amount = 100,
                            name = "Watering Phase 1",
                            order = 0,
                            hours = 24,
                            minutes = 0,
                            repetitions = 2
                        }
                    }
                }
            }
        };

        return JsonContent.Create(root);
    }

    private void SetupHttpGetResponse(HttpContent content)
    {
        httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(
                new HttpResponseMessage(HttpStatusCode.OK)
                { Content = content });
    }

    private Schedule CreateExpectedSchedule()
    {
        return new Schedule
        {
            Trays = new List<Schedule.Tray>
            {
                new Schedule.Tray
                {
                    Name = "Basil",
                    LightingCommands = new List<Schedule.Tray.LightingCommand>
                    {
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 24, 12, 30, 0),
                            LightIntensity = 1
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 24, 18, 30, 0),
                            LightIntensity = 2
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 25, 00, 30, 0),
                            LightIntensity = 3
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 25, 4, 30, 0),
                            LightIntensity = 0
                        },


                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 25, 12, 30, 0),
                            LightIntensity = 1
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 25, 18, 30, 0),
                            LightIntensity = 2
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 26, 00, 30, 0),
                            LightIntensity = 3
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 26, 4, 30, 0),
                            LightIntensity = 0
                        },


                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 26, 12, 30, 0),
                            LightIntensity = 1
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 26, 18, 30, 0),
                            LightIntensity = 2
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 26, 20, 30, 0),
                            LightIntensity = 1
                        },
                        new Schedule.Tray.LightingCommand
                        {
                            At = new DateTime(2022, 1, 27, 2, 30, 0),
                            LightIntensity = 2
                        }
                    },
                    WateringCommands = new List<Schedule.Tray.WateringCommand>
                    {
                        new Schedule.Tray.WateringCommand
                        {
                            At = new DateTime(2022, 1, 24, 12, 30, 0),
                            Amount = 100,
                            Duration = new TimeSpan(24, 0, 0)
                        },
                        new Schedule.Tray.WateringCommand
                        {
                            At = new DateTime(2022, 1, 25, 12, 30, 0),
                            Amount = 100,
                            Duration = new TimeSpan(24, 0, 0)
                        },
                        new Schedule.Tray.WateringCommand
                        {
                            At = new DateTime(2022, 1, 26, 12, 30, 0),
                            Amount = 50,
                            Duration = new TimeSpan(12, 0, 0)
                        },
                        new Schedule.Tray.WateringCommand
                        {
                            At = new DateTime(2022, 1, 27, 0, 30, 0),
                            Amount = 50,
                            Duration = new TimeSpan(12, 0, 0)
                        }

                    }
                }
            }
        };
    }
}