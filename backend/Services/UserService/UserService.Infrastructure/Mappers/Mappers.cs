namespace UserService.Infrastructure.Mappers;

public static class Mappers
{
    public static Domain.Entities.User Map(this Database.Entities.User user)
        => new(user.Id, user.Email, user.FirstName, user.LastName,  user.CreatedAt, user.UpdatedAt);

    public static Database.Entities.User Map(this Domain.Entities.User user)
        => new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

    public static Domain.Entities.Team Map(this Database.Entities.Team team)
        => new(team.Id, team.Name, team.Description, team.CreatedAt, team.UpdatedAt);

    public static Database.Entities.Team Map(this Domain.Entities.Team team)
        => new()
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            CreatedAt = team.CreatedAt,
            UpdatedAt = team.UpdatedAt
        };
    
    public static Domain.Entities.TeamMember MapToTeamMember(this Database.Entities.UserTeamRole userTeamRole)
        => new(userTeamRole.UserId, 
            userTeamRole.TeamId, 
            userTeamRole.User.FirstName, 
            userTeamRole.User.LastName, 
            userTeamRole.User.Email, 
            userTeamRole.Role, 
            userTeamRole.AssignedDate);
    
    public static Domain.Entities.Team MapToTeam(this Database.Entities.UserTeamRole userTeamRole)
        => new(userTeamRole.TeamId, 
            userTeamRole.Team.Name, 
            userTeamRole.Team.Description, 
            userTeamRole.Team.CreatedAt, 
            userTeamRole.Team.UpdatedAt);
}