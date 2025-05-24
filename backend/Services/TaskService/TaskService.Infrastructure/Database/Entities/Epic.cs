using TaskService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities;

[Table("Epics")]
public class Epic
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

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(ProjectId))]
    public Project? Project { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}