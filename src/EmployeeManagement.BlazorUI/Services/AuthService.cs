using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;


namespace EmployeeManagement.BlazorUI.Services;

/// <summary>
/// Implementation of authentication service using JWT tokens.
/// </summary>
public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly CustomAuthStateProvider _authStateProvider;
    private const string TokenKey = "jwtToken";

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var loginRequest = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse?.Token != null)
                {
                    // Store token in local storage
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, loginResponse.Token);
                    
                    // Notify authentication state changed
                    _authStateProvider.NotifyUserAuthentication(loginResponse.Token);
                    
                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        // Remove token from local storage
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        
        // Notify authentication state changed
        _authStateProvider.NotifyUserLogout();
    }

    public async Task<string?> GetCurrentUserAsync()
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
            if (string.IsNullOrEmpty(token))
                return null;

            // Parse JWT to get username (simplified - in production use a JWT library)
            var payload = token.Split('.')[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var json = System.Text.Encoding.UTF8.GetString(jsonBytes);
            var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            
            return claims?.ContainsKey("unique_name") == true 
                ? claims["unique_name"].GetString() 
                : null;
        }
        catch
        {
            return null;
        }
    }

    private static string PadBase64(string base64)
    {
        var padding = 3 - ((base64.Length + 3) % 4);
        if (padding == 0) return base64;
        return base64 + new string('=', padding);
    }

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
