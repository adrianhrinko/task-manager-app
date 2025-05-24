using TaskService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities;

[Table("TaskItems")]
public class TaskItem
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public State State { get; set; } = State.New;

    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public Guid EpicId { get; set; }

    public TimeSpan? Duration { get; set; }

    public DateTime? ScheduledAt { get; set; }

    public DateTime? Due { get; set; }

    public Guid? AssignedToId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Project? Project { get; set; }

    public User? AssignedTo { get; set; }
}