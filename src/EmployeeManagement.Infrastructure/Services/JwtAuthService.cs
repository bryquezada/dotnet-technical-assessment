using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Infrastructure.Services;

/// <summary>
/// Implementation of IAuthService for JWT-based authentication.
/// Handles user authentication and JWT token generation.
/// </summary>
public class JwtAuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public JwtAuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Authenticates a user with username and password.
    /// Returns JWT token if credentials are valid.
    /// </summary>
    public async Task<LoginResponseDto?> LoginAsync(string username, string password)
    {
        // Find user by username
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null)
        {
            return null; // User not found
        }

        // Validate password (plain text comparison for this demo)
        // In production, use password hashing (bcrypt, Argon2, etc.)
        if (user.Password != password)
        {
            return null; // Invalid password
        }

        // Generate JWT token
        var token = GenerateJwtToken(user.Id, user.Username, user.Role);

        // Calculate expiration time
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            ExpiresAt = expiresAt
        };
    }

    /// <summary>
    /// Generates a JWT token for an authenticated user.
    /// Token includes claims: userId, username, role.
    /// </summary>
    public string GenerateJwtToken(int userId, string username, string role)
    {
        // Get JWT configuration from appsettings.json
        var secretKey = _configuration["Jwt:SecretKey"] 
            ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = _configuration["Jwt:Issuer"] ?? "EmployeeManagementAPI";
        var audience = _configuration["Jwt:Audience"] ?? "EmployeeManagementClient";
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");

        // Create security key from secret
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Define claims (user information embedded in token)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()) // Issued at
        };

        // Create JWT token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        // Serialize token to string
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
