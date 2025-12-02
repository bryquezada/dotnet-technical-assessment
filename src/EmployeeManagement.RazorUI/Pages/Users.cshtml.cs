using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.RazorUI.Pages;

/// <summary>
/// Page model for the Users page.
/// Displays list of users retrieved from protected API endpoint using JWT authentication.
/// </summary>
public class UsersModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<UsersModel> _logger;

    public List<UserDto> Users { get; set; } = new();
    public string CurrentUsername { get; set; } = string.Empty;

    [TempData]
    public string? ErrorMessage { get; set; }

    public UsersModel(IHttpClientFactory httpClientFactory, ILogger<UsersModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests to the users page.
    /// Retrieves JWT token from session and calls protected API endpoint.
    /// </summary>
    public async Task<IActionResult> OnGetAsync()
    {
        // Check if user is logged in (has JWT token in session)
        var token = HttpContext.Session.GetString("JwtToken");
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("No JWT token found in session, redirecting to login");
            TempData["InfoMessage"] = "Please log in to access this page.";
            return RedirectToPage("/Login");
        }

        // Get current username from session
        CurrentUsername = HttpContext.Session.GetString("Username") ?? "User";

        try
        {
            _logger.LogInformation("Fetching users from API");

            // Create HTTP client
            var client = _httpClientFactory.CreateClient("EmployeeManagementAPI");

            // Add JWT token to Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Call protected API endpoint
            var response = await client.GetAsync("/api/auth/users");

            if (response.IsSuccessStatusCode)
            {
                // Parse response
                var responseContent = await response.Content.ReadAsStringAsync();
                Users = JsonSerializer.Deserialize<List<UserDto>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<UserDto>();

                _logger.LogInformation("Successfully retrieved {Count} users", Users.Count);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Token is invalid or expired
                _logger.LogWarning("JWT token is invalid or expired");
                HttpContext.Session.Clear();
                TempData["ErrorMessage"] = "Your session has expired. Please log in again.";
                return RedirectToPage("/Login");
            }
            else
            {
                ErrorMessage = $"Error retrieving users: {response.StatusCode}";
                _logger.LogError("API returned status code: {StatusCode}", response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = "Unable to connect to the API. Please ensure the API is running.";
            _logger.LogError(ex, "HTTP request exception while fetching users");
        }
        catch (Exception ex)
        {
            ErrorMessage = "An unexpected error occurred while retrieving users.";
            _logger.LogError(ex, "Unexpected error while fetching users");
        }

        return Page();
    }

    /// <summary>
    /// Handles logout action.
    /// Clears session and redirects to login page.
    /// </summary>
    public IActionResult OnPostLogout()
    {
        _logger.LogInformation("User {Username} logging out", CurrentUsername);
        
        // Clear session
        HttpContext.Session.Clear();
        
        TempData["InfoMessage"] = "You have been successfully logged out.";
        return RedirectToPage("/Login");
    }

    /// <summary>
    /// DTO for User data from API.
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
