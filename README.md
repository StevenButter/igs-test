# igs-test
## Running the app
The schedule generator requires the Recipe API to be running. 

Run the schedule generator API via `dotnet run --project .\ScheduleGenerator.Controllers\`. The app will listen on http://localhost:5120. POST a request http://localhost:5120/schedule with the following body to generate a schedule:
```
{
  "input": [
    {
      "trayNumber": 1,
      "recipeName": "Basil",
      "startDate": "2022-01-24T12:30:00.0000000Z"
    },
    {
      "trayNumber": 3,
      "recipeName": "Strawberries",
      "startDate": "2030-01-01T23:45:00.0000000Z"
    }
  ]
}
```
The API will return a schedule for each tray's lighting and watering commands

The request igs.postman_collection.json can be imported into Postman to test the API

### Testing
Unit tests can be run via `dotnet test .\ScheduleGenerator.Business.Test\`

Integration tests can be run via `dotnet test .\ScheduleGenerator.Controllers.Test.Integration\`

## Development Process
I've developed the solution following clean architecture and SOLID principles. 

The bulk of the logic is inside .\ScheduleGenerator.Business\ScheduleGenerator.cs in .\ScheduleGenerator.Business\

### Structure
ScheduleGenerator.Entities contains records that both the controllers and business layer will use.

ScheduleGenerator.Business only depends on ScheduleGenerator.Entities. It has one public class for creating a schedule, ScheduleGenerator. This class and its constructors params are instantiated using DI by ScheduleGenerator.Controllers.

ScheduleGenerator.Controllers depends on ScheduleGenerator.Business and ScheduleGenerator.Entities. It creates and starts the web application and configures dependency injection. The ScheduleGenerator class is injected into the controller.

## Shortcuts
There is no error checking ScheduleGenerator.Generate(). If something goes wrong querying the recipe API or parsing the body of the response an exception will be thrown, resulting in a 500 response being sent to the /schedule caller. Nullable is enabled, so it is possible to see where the application is assuming a value is assigned.

There is no docker in this project because it is tightly coupled to the recipe API. To get ScheduleGenerator and RecipeApi to be able to communicate easily they should both be created in the same docker compose file. As these projects are in separate repos it is more challenging to send HTTP requests between them.

## Future work
- unit test ScheduleGenerator.Controllers
- check for errors in business layer and unit test
  - return a response from Business layer to Controllers
- dockerize solution
- enable HTTPS