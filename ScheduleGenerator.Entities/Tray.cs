namespace ScheduleGenerator.Entities;

public record Tray
{
    public int TrayNumber { get; init; } = 0;
    public string RecipeName { get; init; } = "";
    public DateTime StartDate { get; init; } = DateTime.Now;
}
