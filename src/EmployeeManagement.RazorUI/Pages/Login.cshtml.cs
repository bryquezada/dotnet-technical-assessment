using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.RazorUI.Pages;

/// <summary>
/// Page model for the Login page.
/// Handles user authentication via API and JWT token storage.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    [TempData]
    public string? ErrorMessage { get; set; }

    [TempData]
    public string? InfoMessage { get; set; }

    public LoginModel(IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests to the login page.
    /// Checks if user is already logged in.
    /// </summary>
    public IActionResult OnGet()
    {
        // Check if user is already logged in
        var token = HttpContext.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
        {
            _logger.LogInformation("User already logged in, redirecting to Users page");
            return RedirectToPage("/Users");
        }

        return Page();
    }

    /// <summary>
    /// Handles POST requests (login form submission).
    /// Calls the API login endpoint and stores JWT token in session.
    /// </summary>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            _logger.LogInformation("Attempting login for user: {Username}", Username);

            // Create HTTP client
            var client = _httpClientFactory.CreateClient("EmployeeManagementAPI");

            // Prepare login request
            var loginRequest = new
            {
                username = Username,
                password = Password
            };

            var jsonContent = JsonSerializer.Serialize(loginRequest);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Call login API endpoint
            var response = await client.PostAsync("/api/auth/login", httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Parse response
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    // Store JWT token in session
                    HttpContext.Session.SetString("JwtToken", loginResponse.Token);
                    HttpContext.Session.SetString("Username", loginResponse.Username);

                    _logger.LogInformation("Login successful for user: {Username}", Username);

                    // Redirect to Users page
                    return RedirectToPage("/Users");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                ErrorMessage = "Invalid username or password. Please try again.";
                _logger.LogWarning("Failed login attempt for user: {Username}", Username);
            }
            else
            {
                ErrorMessage = "An error occurred during login. Please try again later.";
                _logger.LogError("Login API returned status code: {StatusCode}", response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            ErrorMessage = "Unable to connect to the API. Please ensure the API is running.";
            _logger.LogError(ex, "HTTP request exception during login");
        }
        catch (Exception ex)
        {
            ErrorMessage = "An unexpected error occurred. Please try again.";
            _logger.LogError(ex, "Unexpected error during login");
        }

        return Page();
    }

    /// <summary>
    /// DTO for login response from API.
    /// </summary>
    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
