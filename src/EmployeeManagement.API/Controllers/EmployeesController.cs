using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

/// <summary>
/// RESTful API controller for Employee CRUD operations.
/// Provides endpoints for creating, reading, updating, and deleting employees.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IEmployeeRepository employeeRepository,
        ILogger<EmployeesController> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }

    /// <summary>
    /// GET: api/employees
    /// Retrieves all employees.
    /// </summary>
    /// <returns>List of all employees</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        try
        {
            _logger.LogInformation("Retrieving all employees");
            
            var employees = await _employeeRepository.GetAllAsync();
            
            // Map domain entities to DTOs
            var employeeDtos = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Birthdate = e.Birthdate,
                IdentityNumber = e.IdentityNumber,
                Age = e.Age
            });

            return Ok(employeeDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            return StatusCode(500, "An error occurred while retrieving employees");
        }
    }

    /// <summary>
    /// GET: api/employees/{id}
    /// Retrieves a specific employee by ID.
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>Employee details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving employee with ID: {EmployeeId}", id);
            
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", id);
                return NotFound($"Employee with ID {id} not found");
            }

            // Map domain entity to DTO
            var employeeDto = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Birthdate = employee.Birthdate,
                IdentityNumber = employee.IdentityNumber,
                Age = employee.Age
            };

            return Ok(employeeDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee with ID: {EmployeeId}", id);
            return StatusCode(500, "An error occurred while retrieving the employee");
        }
    }

    /// <summary>
    /// POST: api/employees
    /// Creates a new employee.
    /// </summary>
    /// <param name="createDto">Employee creation data</param>
    /// <returns>Created employee</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto createDto)
    {
        try
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Additional business validation
            if (createDto.Birthdate > DateTime.Today)
            {
                return BadRequest("Birthdate cannot be in the future");
            }

            _logger.LogInformation("Creating new employee: {EmployeeName}", createDto.Name);

            // Map DTO to domain entity
            var employee = new Employee
            {
                Name = createDto.Name,
                Birthdate = createDto.Birthdate,
                IdentityNumber = createDto.IdentityNumber
            };

            var createdEmployee = await _employeeRepository.CreateAsync(employee);

            // Map created entity to DTO
            var employeeDto = new EmployeeDto
            {
                Id = createdEmployee.Id,
                Name = createdEmployee.Name,
                Birthdate = createdEmployee.Birthdate,
                IdentityNumber = createdEmployee.IdentityNumber,
                Age = createdEmployee.Age
            };

            _logger.LogInformation("Employee created successfully with ID: {EmployeeId}", createdEmployee.Id);

            // Return 201 Created with location header
            return CreatedAtAction(nameof(GetById), new { id = employeeDto.Id }, employeeDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            return StatusCode(500, "An error occurred while creating the employee");
        }
    }

    /// <summary>
    /// PUT: api/employees/{id}
    /// Updates an existing employee.
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="updateDto">Updated employee data</param>
    /// <returns>Updated employee</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] UpdateEmployeeDto updateDto)
    {
        try
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Additional business validation
            if (updateDto.Birthdate > DateTime.Today)
            {
                return BadRequest("Birthdate cannot be in the future");
            }

            _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);

            // Check if employee exists
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found for update", id);
                return NotFound($"Employee with ID {id} not found");
            }

            // Map DTO to domain entity
            var employee = new Employee
            {
                Id = id,
                Name = updateDto.Name,
                Birthdate = updateDto.Birthdate,
                IdentityNumber = updateDto.IdentityNumber
            };

            var updatedEmployee = await _employeeRepository.UpdateAsync(employee);

            // Map updated entity to DTO
            var employeeDto = new EmployeeDto
            {
                Id = updatedEmployee.Id,
                Name = updatedEmployee.Name,
                Birthdate = updatedEmployee.Birthdate,
                IdentityNumber = updatedEmployee.IdentityNumber,
                Age = updatedEmployee.Age
            };

            _logger.LogInformation("Employee with ID {EmployeeId} updated successfully", id);

            return Ok(employeeDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Employee with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {EmployeeId}", id);
            return StatusCode(500, "An error occurred while updating the employee");
        }
    }

    /// <summary>
    /// DELETE: api/employees/{id}
    /// Deletes an employee.
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);

            var result = await _employeeRepository.DeleteAsync(id);

            if (!result)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion", id);
                return NotFound($"Employee with ID {id} not found");
            }

            _logger.LogInformation("Employee with ID {EmployeeId} deleted successfully", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID: {EmployeeId}", id);
            return StatusCode(500, "An error occurred while deleting the employee");
        }
    }
}
