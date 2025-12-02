using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces;

/// <summary>
/// Repository interface for Employee entity operations.
/// Defines the contract for data access without specifying implementation details.
/// Follows Repository Pattern to abstract data persistence.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Retrieves all employees from the data store.
    /// </summary>
    /// <returns>Collection of all employees</returns>
    Task<IEnumerable<Employee>> GetAllAsync();

    /// <summary>
    /// Retrieves a specific employee by their unique identifier.
    /// </summary>
    /// <param name="id">The employee's unique ID</param>
    /// <returns>The employee if found, null otherwise</returns>
    Task<Employee?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new employee in the data store.
    /// </summary>
    /// <param name="employee">The employee entity to create</param>
    /// <returns>The created employee with assigned ID</returns>
    Task<Employee> CreateAsync(Employee employee);

    /// <summary>
    /// Updates an existing employee's information.
    /// </summary>
    /// <param name="employee">The employee entity with updated information</param>
    /// <returns>The updated employee</returns>
    Task<Employee> UpdateAsync(Employee employee);

    /// <summary>
    /// Deletes an employee from the data store.
    /// </summary>
    /// <param name="id">The ID of the employee to delete</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    Task<bool> DeleteAsync(int id);
}
