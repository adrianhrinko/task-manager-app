using System.ComponentModel.DataAnnotations;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Infrastructure.Database.Entities;

public class UserTeamRole
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid TeamId { get; set; }
    public Team Team { get; set; }

    [Required]
    public TeamRole Role { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
}