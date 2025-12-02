using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Domain.Entities;

/// <summary>
/// Represents an application user for authentication purposes.
/// Used for JWT-based authentication in the system.
/// </summary>
public class UserApp
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique username for login.
    /// </summary>
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User's password.
    /// NOTE: In production, this should be hashed. For this technical assessment, plain text is acceptable.
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User's role in the system (e.g., "Admin", "User").
    /// Used for authorization and JWT claims.
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = "User";
}
