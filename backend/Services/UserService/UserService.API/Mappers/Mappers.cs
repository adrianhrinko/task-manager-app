using UserService.API.DTOs;
using UserService.API.DTOs.Teams;
using UserService.API.DTOs.Users;
using UserService.Infrastructure.Mappers;

namespace UserService.API.Mappers;

public static class Mappers
{
    public static TeamDto Map(this Domain.Entities.Team team)
        => new()
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            CreatedAt = team.CreatedAt,
            UpdatedAt = team.UpdatedAt
        };
    
    public static Domain.Entities.Team Map(this CreateTeamDto team)
        => new(Guid.Empty, team.Name, team.Description, DateTime.UtcNow, DateTime.UtcNow);
    
    public static Domain.Entities.Team Map(this UpdateTeamDto team)
        => new(team.Id, team.Name, team.Description, DateTime.UtcNow, DateTime.UtcNow);
    
    public static Domain.Entities.User Map(this CreateUserDto user)
        => new(Guid.Empty, user.Email, user.FirstName, user.LastName, DateTime.UtcNow, DateTime.UtcNow);
    
    public static Domain.Entities.User Map(this UpdateUserDto user)
        => new(user.Id, user.Email,  user.FirstName, user.LastName, DateTime.UtcNow, DateTime.UtcNow);

    public static UserDto Map(this Domain.Entities.User user)
        => new()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    
    public static TeamMemberDto Map(this Domain.Entities.TeamMember user)
        => new()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            AssignedDate = user.AssignedDate,
            TeamId = user.TeamId
        };
}