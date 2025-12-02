using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

/// <summary>
/// Controller for authentication and user management.
/// Provides endpoints for login and retrieving users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IUserRepository userRepository,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// POST: api/auth/login
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginRequest">Login credentials (username and password)</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Login attempt for user: {Username}", loginRequest.Username);

            // Attempt authentication
            var loginResponse = await _authService.LoginAsync(loginRequest.Username, loginRequest.Password);

            if (loginResponse == null)
            {
                _logger.LogWarning("Failed login attempt for user: {Username}", loginRequest.Username);
                return Unauthorized("Invalid username or password");
            }

            _logger.LogInformation("Successful login for user: {Username}", loginRequest.Username);

            return Ok(loginResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Username}", loginRequest.Username);
            return StatusCode(500, "An error occurred during login");
        }
    }

    /// <summary>
    /// GET: api/auth/users
    /// Retrieves all users (protected endpoint - requires JWT authentication).
    /// </summary>
    /// <returns>List of all users (without passwords)</returns>
    [HttpGet("users")]
    [Authorize] // Requires valid JWT token
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            _logger.LogInformation("Retrieving all users");

            var users = await _userRepository.GetAllAsync();

            // Map to DTOs (exclude passwords)
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            });

            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, "An error occurred while retrieving users");
        }
    }
}
