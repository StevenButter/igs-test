using ScheduleGenerator.Business.Model;

namespace ScheduleGenerator.Business.Creation;

internal abstract class ScheduleCreator<TPhase, TCommand> where TPhase : Phase
{
    public IEnumerable<TCommand> Create(DateTime startDate, IEnumerable<TPhase> phases)
    {
        List<TCommand> commands = new();

        var orderedPhases = phases.OrderBy(x => x.Order);
        foreach (var phase in orderedPhases)
        {
            for (int i = 0; i < phase.Repetitions; i++)
            {
                var phaseCommands = GetCommands(startDate, phase);
                commands.AddRange(phaseCommands);
                startDate += new TimeSpan(phase.Hours.Value, phase.Minutes.Value, 0);
            }

        }

        return commands;
    }

    protected abstract List<TCommand> GetCommands(DateTime startDate, TPhase phase);
}
