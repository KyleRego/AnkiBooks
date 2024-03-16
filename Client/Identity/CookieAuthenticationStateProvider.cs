using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using AnkiBooks.Client.Identity.Models;
using AnkiBooks.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Client.Identity;

/// <summary>
/// Handles state for cookie-based auth.
/// </summary>
/// <remarks>
/// Create a new instance of the auth provider.
/// </remarks>
/// <param name="httpClientFactory">Factory to retrieve auth client.</param>
public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory)
                : AuthenticationStateProvider, IAccountManagement
{
    /// <summary>
    /// Map the JavaScript-formatted properties to C#-formatted classes.
    /// </summary>
    private readonly JsonSerializerOptions jsonSerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

    /// <summary>
    /// Special auth client.
    /// </summary>
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Auth");

    /// <summary>
    /// Authentication state.
    /// </summary>
    private bool _authenticated = false;

    /// <summary>
    /// Default principal for anonymous (not authenticated) users.
    /// </summary>
    private readonly ClaimsPrincipal Unauthenticated =
        new(new ClaimsIdentity());

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>The result serialized to a <see cref="FormResult"/>.
    /// </returns>
    public async Task<FormResult> RegisterAsync(string email, string password)
    {
        string[] defaultDetail = ["An unknown error prevented registration from succeeding."];

        try
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync(
                "register", new
                {
                    email,
                    password
                });

            if (result.IsSuccessStatusCode)
            {
                return new FormResult { Succeeded = true };
            }

            string details = await result.Content.ReadAsStringAsync();
            JsonDocument problemDetails = JsonDocument.Parse(details);
            List<string> errors = [];
            JsonElement errorList = problemDetails.RootElement.GetProperty("errors");

            foreach (JsonProperty errorEntry in errorList.EnumerateObject())
            {
                if (errorEntry.Value.ValueKind == JsonValueKind.String)
                {
                    errors.Add(errorEntry.Value.GetString()!);
                }
                else if (errorEntry.Value.ValueKind == JsonValueKind.Array)
                {
                    errors.AddRange(
                        errorEntry.Value.EnumerateArray().Select(
                            e => e.GetString() ?? string.Empty)
                        .Where(e => !string.IsNullOrEmpty(e)));
                }
            }

            return new FormResult
            {
                Succeeded = false,
                ErrorList = problemDetails == null ? defaultDetail : [.. errors]
            };
        }
        catch { }

        return new FormResult
        {
            Succeeded = false,
            ErrorList = defaultDetail
        };
    }

    /// <summary>
    /// User login.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>The result of the login request serialized to a <see cref="FormResult"/>.</returns>
    public async Task<FormResult> LoginAsync(string email, string password)
    {
        try
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync(
                "login?useCookies=true", new
                {
                    email,
                    password
                });

            if (result.IsSuccessStatusCode)
            {
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                return new FormResult { Succeeded = true };
            }
        }
        catch { }

        return new FormResult
        {
            Succeeded = false,
            ErrorList = ["Invalid email and/or password."]
        };
    }

    /// <summary>
    /// Get authentication state.
    /// </summary>
    /// <remarks>
    /// Called by Blazor when an authentication-based decision needs to be made, and cached
    /// until the changed state notification is raised.
    /// </remarks>
    /// <returns>The authentication state asynchronous request.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _authenticated = false;

        ClaimsPrincipal userPrincipal = Unauthenticated;

        try
        {
            // the user info endpoint is secured, so if the user isn't logged in this will fail
            // TODO: Is this necessary to implement like this as it causes console messages
            HttpResponseMessage userResponse = await _httpClient.GetAsync("manage/info");
            userResponse.EnsureSuccessStatusCode();

            string userJson = await userResponse.Content.ReadAsStringAsync();
            ApplicationUser? userInfo = JsonSerializer.Deserialize<ApplicationUser>(userJson, jsonSerializerOptions);

            if (userInfo != null && !string.IsNullOrWhiteSpace(userInfo.Email))
            {
                List<Claim> claims =
                    [
                        new(ClaimTypes.Name, userInfo.Email),
                        new(ClaimTypes.Email, userInfo.Email)
                    ];

                HttpResponseMessage rolesResponse = await _httpClient.GetAsync("roles");
                rolesResponse.EnsureSuccessStatusCode();

                string rolesJson = await rolesResponse.Content.ReadAsStringAsync();
                ApplicationRoleClaim[]? roles = JsonSerializer.Deserialize<ApplicationRoleClaim[]>(rolesJson, jsonSerializerOptions);

                if (roles?.Length > 0)
                {
                    foreach (ApplicationRoleClaim role in roles)
                    {
                        if (!string.IsNullOrEmpty(role.ClaimType) && !string.IsNullOrEmpty(role.ClaimValue))
                        {
                            claims.Add(new Claim(role.ClaimType, role.ClaimValue));
                        }
                    }
                }

                ClaimsIdentity id = new(claims, nameof(CookieAuthenticationStateProvider));
                userPrincipal = new ClaimsPrincipal(id);
                _authenticated = true;
            }
        }
        catch { }

        return new AuthenticationState(userPrincipal);
    }

    public async Task LogoutAsync()
    {
        const string Empty = "{}";
        StringContent emptyContent = new(Empty, Encoding.UTF8, "application/json");
        await _httpClient.PostAsync("logout", emptyContent);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<bool> CheckAuthenticatedAsync()
    {
        await GetAuthenticationStateAsync();
        return _authenticated;
    }
}