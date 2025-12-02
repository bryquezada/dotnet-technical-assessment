using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Domain.Entities;

/// <summary>
/// Represents an employee in the system.
/// Core domain entity with business rules and validation.
/// </summary>
public class Employee
{
    /// <summary>
    /// Unique identifier for the employee.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the employee.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Employee's date of birth.
    /// Used for age verification and record keeping.
    /// </summary>
    [Required(ErrorMessage = "Birthdate is required")]
    public DateTime Birthdate { get; set; }

    /// <summary>
    /// Government-issued identity number (e.g., SSN, National ID).
    /// Format: XXX-XXXXXX-XXXXX (El Salvador format example)
    /// </summary>
    [Required(ErrorMessage = "Identity number is required")]
    [StringLength(50, ErrorMessage = "Identity number cannot exceed 50 characters")]
    public string IdentityNumber { get; set; } = string.Empty;

    /// <summary>
    /// Calculated property: Employee's current age based on birthdate.
    /// </summary>
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - Birthdate.Year;
            if (Birthdate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
