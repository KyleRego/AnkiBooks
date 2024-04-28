using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using IUserIdProvider = AnkiBooks.ApplicationCore.Interfaces.Services.IUserIdProvider;

namespace AnkiBooks.WebApp.Services;

public class UserIdProvider(AuthenticationStateProvider authStateProvider) : IUserIdProvider
{
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

    public async Task<string?> GetCurrentUserId()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync())
                    .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}