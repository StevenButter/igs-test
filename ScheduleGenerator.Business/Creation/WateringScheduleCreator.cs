using ScheduleGenerator.Business.Model;
using ScheduleGenerator.Entities;

namespace ScheduleGenerator.Business.Creation;

internal sealed class WateringScheduleCreator : ScheduleCreator<WateringPhase, Schedule.Tray.WateringCommand>
{
    protected override List<Schedule.Tray.WateringCommand> GetCommands(DateTime startDate, WateringPhase phase)
    {
        return new List<Schedule.Tray.WateringCommand>
            {
                new Schedule.Tray.WateringCommand
                {
                At = startDate,
                Amount = phase.Amount.Value,
                Duration = new TimeSpan(phase.Hours.Value, phase.Minutes.Value, 0)
                }
            };
    }
}


