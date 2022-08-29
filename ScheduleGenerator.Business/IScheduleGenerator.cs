using ScheduleGenerator.Entities;

namespace ScheduleGenerator.Business;

public interface IScheduleGenerator
{
    Task<Schedule> Generate(IEnumerable<Tray> tray);
}