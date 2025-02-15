namespace UserService.Infrastructure.Mappers;

public static class Mappers
{
    public static Domain.Entities.User Map(this Database.Entities.User user)
        => new(user.Id, user.FirstName, user.LastName, user.Email, user.CreatedAt, user.UpdatedAt);

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
}