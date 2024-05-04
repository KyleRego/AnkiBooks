using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Services;

public class UserArticleService(IUserArticleRepository repository,
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

    public Task<Article?> PostUserArticle(Article article)
    {
        throw new NotImplementedException();
    }

    public Task<Article?> PatchUserArticle(Article article)
    {
        throw new NotImplementedException();
    }
}