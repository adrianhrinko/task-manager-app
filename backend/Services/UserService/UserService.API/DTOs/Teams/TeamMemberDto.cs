using UserService.API.DTOs.Users;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.API.DTOs.Teams;

public class TeamMemberDto: UserBaseDto
{
    public Guid TeamId { get; set; }
    public TeamRole Role { get; set; }
    public DateTime AssignedDate { get; set; }
}