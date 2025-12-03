namespace EmployeeManagement.BlazorUI.Services;

/// <summary>
/// Service interface for authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user with username and password.
    /// </summary>
    Task<bool> LoginAsync(string username, string password);
    
    /// <summary>
    /// Logs out the current user.
    /// </summary>
    Task LogoutAsync();
    
    /// <summary>
    /// Gets the current user's username from the stored token.
    /// </summary>
    Task<string?> GetCurrentUserAsync();
}
