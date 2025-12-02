using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

/// <summary>
/// Repository interface for UserApp entity operations.
/// Defines the contract for user data access.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves all users from the data store.
    /// </summary>
    /// <returns>Collection of all users</returns>
    Task<IEnumerable<UserApp>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific user by their username.
    /// Used for authentication purposes.
    /// </summary>
    /// <param name="username">The username to search for</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<UserApp?> GetByUsernameAsync(string username);
}
