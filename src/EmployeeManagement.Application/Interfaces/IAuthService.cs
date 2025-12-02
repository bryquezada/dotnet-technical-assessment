using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

/// <summary>
/// Service interface for authentication operations.
/// Handles user authentication and JWT token generation.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with username and password.
    /// </summary>
    /// <param name="username">Username for authentication</param>
    /// <param name="password">Password for authentication</param>
    /// <returns>Login response with JWT token if successful, null otherwise</returns>
    Task<LoginResponseDto?> LoginAsync(string username, string password);

    /// <summary>
    /// Generates a JWT token for an authenticated user.
    /// </summary>
    /// <param name="userId">User's unique identifier</param>
    /// <param name="username">User's username</param>
    /// <param name="role">User's role</param>
    /// <returns>JWT token string</returns>
    string GenerateJwtToken(int userId, string username, string role);
}
