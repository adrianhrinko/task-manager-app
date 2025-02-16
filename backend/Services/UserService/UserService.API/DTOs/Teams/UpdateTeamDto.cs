using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs;

public class UpdateTeamDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}