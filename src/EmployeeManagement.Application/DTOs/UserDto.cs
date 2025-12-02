namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for User responses.
/// Excludes sensitive information like passwords.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Username of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Role of the user in the system.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
