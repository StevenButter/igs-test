using Microsoft.AspNetCore.Mvc;
using ScheduleGenerator.Business;
using ScheduleGenerator.Entities;

namespace ScheduleGeneratorApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleGenerator scheduleGenerator;

    public ScheduleController(IScheduleGenerator scheduleGenerator)
    {
        this.scheduleGenerator = scheduleGenerator;
    }

    [HttpPost]
    public async Task<ActionResult<Schedule>> Post(IEnumerable<Tray> trays)
    {
        return await scheduleGenerator.Generate(trays).ConfigureAwait(false);
    }
}