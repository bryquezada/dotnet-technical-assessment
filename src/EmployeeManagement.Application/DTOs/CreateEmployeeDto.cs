using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for creating new employees.
/// Contains validation rules for employee creation requests.
/// </summary>
public class CreateEmployeeDto
{
    /// <summary>
    /// Full name of the employee.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Employee's date of birth.
    /// Must be a past date (cannot be born in the future).
    /// </summary>
    [Required(ErrorMessage = "Birthdate is required")]
    public DateTime Birthdate { get; set; }

    /// <summary>
    /// Government-issued identity number.
    /// Must be unique in the system.
    /// </summary>
    [Required(ErrorMessage = "Identity number is required")]
    [StringLength(50, ErrorMessage = "Identity number cannot exceed 50 characters")]
    public string IdentityNumber { get; set; } = string.Empty;
}
