using System.Security.Claims;
using System.Security.Principal;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.WebApp.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace AnkiBooks.WebApp.Services;

public class ServerUserArticleService(  IUserArticleRepository repository,
                                        AuthenticationStateProvider authStateProvider) : IUserArticleService
{
    private readonly IUserArticleRepository _repository = repository;
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

    private async Task<string?> GetCurrentUserId()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync())
                    .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<Article?> GetUserArticle(string articleId)
    {
        string? userId = await GetCurrentUserId();
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetArticleAsync(userId, articleId);
    }

    public async Task<List<Article>?> GetUserArticles()
    {
        string? userId = await GetCurrentUserId();
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetArticlesAsync(userId);
    }

    public Task<Article?> PostUserArticle(Article articleData)
    {
        throw new NotImplementedException();
    }
}