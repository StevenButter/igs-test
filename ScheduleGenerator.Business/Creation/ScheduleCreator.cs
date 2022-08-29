using ScheduleGenerator.Business.Model;

namespace ScheduleGenerator.Business.Creation;

internal abstract class ScheduleCreator<TPhase, TCommand> where TPhase : Phase
{
    public IEnumerable<TCommand> Create(DateTime startDate, IEnumerable<TPhase> phases)
    {
        return phases.OrderBy(x => x.Order)
            .SelectMany(phase =>
            {
                List<TCommand> phaseCommands = new();
                for (int i = 0; i < phase.Repetitions; i++)
                {
                    phaseCommands.AddRange(GetCommands(startDate, phase));
                    startDate += new TimeSpan(phase.Hours.Value, phase.Minutes.Value, 0);
                }
                return phaseCommands;
            });
    }

    protected abstract List<TCommand> GetCommands(DateTime startDate, TPhase phase);
}
