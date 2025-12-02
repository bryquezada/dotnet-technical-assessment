using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of IEmployeeRepository.
/// Uses a List to store employees in memory (data is lost on application restart).
/// Thread-safe operations using lock for concurrent access.
/// Includes seed data with 3 pre-loaded employees.
/// </summary>
public class InMemoryEmployeeRepository : IEmployeeRepository
{
    // In-memory storage using List
    private readonly List<Employee> _employees;
    private readonly object _lock = new object();
    private int _nextId;

    /// <summary>
    /// Constructor initializes the repository with seed data.
    /// </summary>
    public InMemoryEmployeeRepository()
    {
        // Initialize with seed data - 3 employees as required
        _employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "John Doe",
                Birthdate = new DateTime(1990, 5, 15),
                IdentityNumber = "001-150590-1001A"
            },
            new Employee
            {
                Id = 2,
                Name = "Jane Smith",
                Birthdate = new DateTime(1985, 8, 22),
                IdentityNumber = "001-220885-2002B"
            },
            new Employee
            {
                Id = 3,
                Name = "Bob Johnson",
                Birthdate = new DateTime(1992, 12, 10),
                IdentityNumber = "001-101292-3003C"
            }
        };

        _nextId = 4; // Next available ID
    }

    /// <summary>
    /// Retrieves all employees from in-memory storage.
    /// </summary>
    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        lock (_lock)
        {
            // Return a copy to prevent external modifications
            return Task.FromResult<IEnumerable<Employee>>(_employees.ToList());
        }
    }

    /// <summary>
    /// Retrieves a specific employee by ID.
    /// </summary>
    public Task<Employee?> GetByIdAsync(int id)
    {
        lock (_lock)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(employee);
        }
    }

    /// <summary>
    /// Creates a new employee and assigns a unique ID.
    /// </summary>
    public Task<Employee> CreateAsync(Employee employee)
    {
        lock (_lock)
        {
            // Assign new ID
            employee.Id = _nextId++;
            
            // Add to in-memory list
            _employees.Add(employee);
            
            return Task.FromResult(employee);
        }
    }

    /// <summary>
    /// Updates an existing employee's information.
    /// </summary>
    public Task<Employee> UpdateAsync(Employee employee)
    {
        lock (_lock)
        {
            var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);
            
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {employee.Id} not found");
            }

            // Update properties
            existingEmployee.Name = employee.Name;
            existingEmployee.Birthdate = employee.Birthdate;
            existingEmployee.IdentityNumber = employee.IdentityNumber;

            return Task.FromResult(existingEmployee);
        }
    }

    /// <summary>
    /// Deletes an employee from in-memory storage.
    /// </summary>
    public Task<bool> DeleteAsync(int id)
    {
        lock (_lock)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == id);
            
            if (employee == null)
            {
                return Task.FromResult(false);
            }

            _employees.Remove(employee);
            return Task.FromResult(true);
        }
    }
}
