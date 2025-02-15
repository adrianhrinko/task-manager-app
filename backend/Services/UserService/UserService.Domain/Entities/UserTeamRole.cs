using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class UserTeamRole
{
    public Guid UserId { get; set; }
    public Guid TeamId { get; set; }
    public TeamRole Role { get; set; }
    public DateTime AssignedDate { get; set; }
}