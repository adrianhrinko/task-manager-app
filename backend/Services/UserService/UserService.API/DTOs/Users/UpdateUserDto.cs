using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.Users;

public class UpdateUserDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
}