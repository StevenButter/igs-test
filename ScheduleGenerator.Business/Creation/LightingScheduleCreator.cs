using ScheduleGenerator.Business.Model;
using ScheduleGenerator.Entities;

namespace ScheduleGenerator.Business.Creation;

internal sealed class LightingScheduleCreator : ScheduleCreator<LightingPhase, Schedule.Tray.LightingCommand>
{
    protected override List<Schedule.Tray.LightingCommand> GetCommands(DateTime startDate, LightingPhase phase)
    {
        return phase.Operations.Select(operation =>
        {
            var offsetHours = operation.OffsetHours;
            var offsetMinutes = operation.OffsetMinutes;

            TimeSpan commandOffset = new(offsetHours.Value, offsetMinutes.Value, 0);
            var commandStart = startDate + commandOffset;

            return new Schedule.Tray.LightingCommand
            {
                At = commandStart,
                LightIntensity = operation.LightIntensity.Value
            };

        })
        .ToList();
    }

}