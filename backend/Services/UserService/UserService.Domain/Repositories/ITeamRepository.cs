using Shared.Domain;

namespace UserService.Domain.Repositories;

public interface ITeamRepository
{
    /// <summary>
    /// Retrieves a team by its unique identifier
    /// </summary>
    /// <param name="id">The GUID of the team to retrieve</param>
    /// <returns>The team if found, null otherwise</returns>
    Task<Entities.Team?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a paged list of all teams
    /// </summary>
    /// <param name="query">The query parameters for filtering, sorting and pagination</param>
    /// <returns>A paginated list of teams</returns>
    Task<PagedList<Entities.Team>> GetAllAsync(Query query);

    /// <summary>
    /// Creates a new team in the system
    /// </summary>
    /// <param name="userId">The ID of the user creating the team</param>
    /// <param name="team">The team entity to create</param>
    /// <returns>The created team with generated ID</returns>
    Task<Entities.Team> CreateAsync(Guid userId, Entities.Team team);

    /// <summary>
    /// Updates an existing team's information
    /// </summary>
    /// <param name="team">The team entity with updated information</param>
    /// <returns>The updated team</returns>
    Task<Entities.Team> UpdateAsync(Entities.Team team);

    /// <summary>
    /// Deletes a team from the system
    /// </summary>
    /// <param name="id">The GUID of the team to delete</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a team exists by its ID
    /// </summary>
    /// <param name="id">The GUID to check</param>
    /// <returns>True if the team exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id);
}