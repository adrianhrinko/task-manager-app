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
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a user by their email address
    /// </summary>
    /// <param name="email">The email address to search for</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Retrieves a paged list of all users 
    /// </summary>
    /// <param name="query">The query parameters for filtering, sorting and pagination</param>
    /// <returns>A paginated list of users</returns>
    Task<PagedList<User>> GetAllAsync(Query query);

    /// <summary>
    /// Creates a new user in the system
    /// </summary>
    /// <param name="user">The user entity to create</param>
    /// <returns>The created user with generated ID</returns>
    Task<User> CreateAsync(User user);

    /// <summary>
    /// Updates an existing user's information
    /// </summary>
    /// <param name="user">The user entity with updated information</param>
    /// <returns>The updated user</returns>
    Task<User> UpdateAsync(User user);

    /// <summary>
    /// Deletes a user from the system
    /// </summary>
    /// <param name="id">The GUID of the user to delete</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a user exists by their ID
    /// </summary>
    /// <param name="id">The GUID to check</param>
    /// <returns>True if the user exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    /// Checks if a user exists by their email address
    /// </summary>
    /// <param name="email">The email address to check</param>
    /// <returns>True if a user with the email exists, false otherwise</returns>
    Task<bool> ExistsByEmailAsync(string email);
}