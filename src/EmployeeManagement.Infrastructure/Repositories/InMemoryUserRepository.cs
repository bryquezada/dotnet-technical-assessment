using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces;

namespace EmployeeManagement.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of IUserRepository.
/// Uses a List to store users in memory (data is lost on application restart).
/// Thread-safe operations using lock for concurrent access.
/// Includes seed data with 3 pre-loaded users for testing.
/// </summary>
public class InMemoryUserRepository : IUserRepository
{
    // In-memory storage using List
    private readonly List<UserApp> _users;
    private readonly object _lock = new object();

    /// <summary>
    /// Constructor initializes the repository with seed data.
    /// NOTE: Passwords are stored in plain text for this technical assessment.
    /// In production, passwords should be hashed using bcrypt or similar.
    /// </summary>
    public InMemoryUserRepository()
    {
        // Initialize with seed data - 3 users as required
        _users = new List<UserApp>
        {
            new UserApp
            {
                Id = 1,
                Username = "admin",
                Password = "admin123", // Plain text for demo purposes
                Role = "Admin"
            },
            new UserApp
            {
                Id = 2,
                Username = "user1",
                Password = "user123",
                Role = "User"
            },
            new UserApp
            {
                Id = 3,
                Username = "test",
                Password = "test123",
                Role = "User"
            }
        };
    }

    /// <summary>
    /// Retrieves all users from in-memory storage.
    /// </summary>
    public Task<IEnumerable<UserApp>> GetAllAsync()
    {
        lock (_lock)
        {
            // Return a copy to prevent external modifications
            return Task.FromResult<IEnumerable<UserApp>>(_users.ToList());
        }
    }

    /// <summary>
    /// Retrieves a specific user by username.
    /// Used for authentication purposes.
    /// </summary>
    public Task<UserApp?> GetByUsernameAsync(string username)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }
    }
}
