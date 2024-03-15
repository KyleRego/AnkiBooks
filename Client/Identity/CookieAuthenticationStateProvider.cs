using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Client.Identity.Models;
using System.Text;

namespace Client.Identity;

/// <summary>
/// Handles state for cookie-based auth.
/// </summary>
/// <remarks>
/// Create a new instance of the auth provider.
/// </remarks>
/// <param name="httpClientFactory">Factory to retrieve auth client.</param>
public class CookieAuthenticationStateProvider(IHttpClientFactory httpClientFactory) : AuthenticationStateProvider, IAccountManagement
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

        ClaimsPrincipal user = Unauthenticated;

        try
        {
            // the user info endpoint is secured, so if the user isn't logged in this will fail
            // TODO: Is this necessary to implement like this as it causes console messages
            HttpResponseMessage userResponse = await _httpClient.GetAsync("manage/info");

            // throw if user info wasn't retrieved
            userResponse.EnsureSuccessStatusCode();

            // user is authenticated,so let's build their authenticated identity
            string userJson = await userResponse.Content.ReadAsStringAsync();
            UserInfo? userInfo = JsonSerializer.Deserialize<UserInfo>(userJson, jsonSerializerOptions);

            if (userInfo != null)
            {
                // in our system name and email are the same
                List<Claim> claims =
                    [
                        new(ClaimTypes.Name, userInfo.Email),
                        new(ClaimTypes.Email, userInfo.Email),
                        // add any additional claims
                        .. userInfo.Claims.Where(c => c.Key != ClaimTypes.Name && c.Key != ClaimTypes.Email)
                            .Select(c => new Claim(c.Key, c.Value)),
                    ];

                // tap the roles endpoint for the user's roles
                HttpResponseMessage rolesResponse = await _httpClient.GetAsync("roles");
                rolesResponse.EnsureSuccessStatusCode();
                string rolesJson = await rolesResponse.Content.ReadAsStringAsync();
                RoleClaim[]? roles = JsonSerializer.Deserialize<RoleClaim[]>(rolesJson, jsonSerializerOptions);

                if (roles?.Length > 0)
                {
                    foreach (RoleClaim role in roles)
                    {
                        if (!string.IsNullOrEmpty(role.Type) && !string.IsNullOrEmpty(role.Value))
                        {
                            claims.Add(new Claim(role.Type, role.Value, role.ValueType, role.Issuer, role.OriginalIssuer));
                        }
                    }
                }

                // set the principal
                ClaimsIdentity id = new(claims, nameof(CookieAuthenticationStateProvider));
                user = new ClaimsPrincipal(id);
                _authenticated = true;
            }
        }
        catch { }

        return new AuthenticationState(user);
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

    public class RoleClaim
    {
        public string? Issuer { get; set; }
        public string? OriginalIssuer { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
        public string? ValueType { get; set; }
    }
}