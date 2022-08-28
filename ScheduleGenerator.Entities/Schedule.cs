namespace ScheduleGenerator.Entities;

public sealed class Schedule
{
    public sealed class Tray
    {
        public sealed record LightingCommand
        {
            public DateTime At { get; init; }
            public int LightIntensity { get; init; }
        }

        public sealed record WateringCommand
        {
            public DateTime At { get; init; }
            public int Amount { get; init; }
            public TimeSpan Duration { get; init; }
        }


        public string Name { get; init; }

        public IEnumerable<LightingCommand> LightingCommands { get; init; }
        public IEnumerable<WateringCommand> WateringCommands { get; init; }
    }

    public IEnumerable<Tray> Trays { get; init; }
}


