using ScheduleGenerator.Business.Model;
using ScheduleGenerator.Entities;

namespace ScheduleGenerator.Business.Creation;

internal sealed class LightingScheduleCreator : ScheduleCreator<LightingPhase, Schedule.Tray.LightingCommand>
{
    protected override List<Schedule.Tray.LightingCommand> GetCommands(DateTime startDate, LightingPhase phase)
    {
        List<Schedule.Tray.LightingCommand> commands = new();
        foreach (var operation in phase.Operations)
        {
            var offsetHours = operation.OffsetHours;
            var offsetMinutes = operation.OffsetMinutes;

            TimeSpan commandOffset = new(offsetHours.Value, offsetMinutes.Value, 0);
            var commandStart = startDate + commandOffset;

            Schedule.Tray.LightingCommand command = new()
            {
                At = commandStart,
                LightIntensity = operation.LightIntensity.Value
            };

            commands.Add(command);
        }

        return commands;
    }

}