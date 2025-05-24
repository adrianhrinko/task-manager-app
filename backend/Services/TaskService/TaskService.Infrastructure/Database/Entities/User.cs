using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities;

[Table("Users")]
public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    
    public ICollection<Project> Projects { get; set; } = new List<Project>();

}