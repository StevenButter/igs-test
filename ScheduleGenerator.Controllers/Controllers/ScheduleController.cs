using Microsoft.AspNetCore.Mvc;
using ScheduleGenerator.Business;
using ScheduleGenerator.Entities;
using ScheduleGeneratorApi.Model;

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
    public async Task<ActionResult<Schedule>> Post(Input input)
    {
        return await scheduleGenerator.Generate(input.Trays).ConfigureAwait(false);
    }
}