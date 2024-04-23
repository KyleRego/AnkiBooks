using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.WebApp.Services;

public class ServerUserArticleService(IUserArticleRepository repository) : IUserArticleService
{
    private readonly IUserArticleRepository _repository = repository;

    public async Task<Article?> GetUserArticle(string articleId, string? userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetArticleAsync(userId, articleId);
    }

    public async Task<List<Article>?> GetUserArticles(string? userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetArticlesAsync(userId);
    }

    public Task<Article?> PostUserArticle(Article articleData)
    {
        throw new NotImplementedException();
    }
}