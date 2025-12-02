namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for Employee responses.
/// Used to transfer employee data from API to clients.
/// Separates domain entities from API contracts.
/// </summary>
public class EmployeeDto
{
    /// <summary>
    /// Unique identifier for the employee.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the employee.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Employee's date of birth.
    /// </summary>
    public DateTime Birthdate { get; set; }

    /// <summary>
    /// Government-issued identity number.
    /// </summary>
    public string IdentityNumber { get; set; } = string.Empty;

    /// <summary>
    /// Calculated age of the employee.
    /// </summary>
    public int Age { get; set; }
}
