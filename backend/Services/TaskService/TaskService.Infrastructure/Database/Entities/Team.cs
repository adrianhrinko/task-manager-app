using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskService.Infrastructure.Database.Entities;

[Table("Teams")]
public class Team
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}