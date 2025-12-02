namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for login responses.
/// Contains JWT token and user information.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// JWT token for authentication.
    /// Should be included in Authorization header as "Bearer {token}".
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Username of the authenticated user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration date and time (UTC).
    /// </summary>
    public DateTime ExpiresAt { get; set; }
}
