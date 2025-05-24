using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class TeamMember(
    Guid userId,
    Guid teamId,
    string firstName,
    string lastName,
    string email,
    TeamRole role,
    DateTime assignedDate) : Shared.Domain.User(userId, firstName, lastName, email)
{
    public Guid TeamId { get; set; } = teamId;
    public TeamRole Role { get; set; } = role;
    public DateTime AssignedDate { get; set; } = assignedDate;
}