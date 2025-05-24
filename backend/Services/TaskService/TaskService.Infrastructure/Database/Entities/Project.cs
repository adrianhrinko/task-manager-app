using TaskService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities;

[Table("Projects")]
public class Project
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

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? TeamId { get; set; }

    public Guid? OwnerId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    
    public ICollection<Epic> Epics { get; set; } = new List<Epic>();

}