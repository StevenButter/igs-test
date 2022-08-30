using System.ComponentModel.DataAnnotations;

namespace ScheduleGenerator.Entities;

public record Tray
{
    [Required]
    public int? TrayNumber { get; init; }

    [Required]
    public string? RecipeName { get; init; }

    [Required]
    public DateTime? StartDate { get; init; }
}
