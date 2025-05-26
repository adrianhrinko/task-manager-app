using Shared.Domain;
using UserService.Domain.Entities;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier
    /// </summary>
    /// <param name="id">The GUID of the user to retrieve</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<Entities.User?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<Entities.User?> GetByEmailAsync(string email, CancellationToken ct);

    /// <summary>
    /// Retrieves a paged list of all users 
    /// </summary>
    /// <param name="query">The query parameters for filtering, sorting and pagination</param>
    /// <returns>A paginated list of users</returns>
    Task<PagedList<Entities.User>> GetAllAsync(Query query, CancellationToken ct);

    /// <summary>
    /// Retrieves a paged list of teams that a user is a member of 
    /// </summary>
    /// <param name="userId">The ID of user</param>
    /// <param name="query">The query parameters for filtering, sorting and pagination</param>
    /// <returns>A paginated list of teams</returns>
    Task<PagedList<Team>> GetTeamsAsync(Guid userId, Query query, CancellationToken ct);

    /// <summary>
    /// Creates a new user in the system
    /// </summary>
    /// <param name="user">The user entity to create</param>
    /// <returns>The created user with generated ID</returns>
    Task<Entities.User> CreateAsync(Entities.User user, CancellationToken ct);

    /// <summary>
    /// Updates an existing user's information
    /// </summary>
    /// <param name="user">The user entity with updated information</param>
    /// <returns>The updated user</returns>
    Task<Entities.User> UpdateAsync(Entities.User user, CancellationToken ct);

    /// <summary>
    /// Deletes a user from the system
    /// </summary>
    /// <param name="id">The GUID of the user to delete</param>
    Task DeleteAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Checks if a user exists by their ID
    /// </summary>
    /// <param name="id">The GUID to check</param>
    /// <returns>True if the user exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Checks if a user exists by their email address
    /// </summary>
    /// <param name="email">The email address to check</param>
    /// <returns>True if a user with the email exists, false otherwise</returns>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
}