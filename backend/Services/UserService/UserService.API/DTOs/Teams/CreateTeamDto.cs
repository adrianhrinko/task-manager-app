using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Teams;

public class CreateTeamDto
{
    [Required]
    public Guid CreatedBy { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}