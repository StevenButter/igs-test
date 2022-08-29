using Microsoft.AspNetCore.Mvc;
using ScheduleGenerator.Entities;

namespace ScheduleGeneratorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{
    [HttpPost]
    public ActionResult<Schedule> Post(IEnumerable<Tray> trays)
    {
        return new Schedule();
    }
}