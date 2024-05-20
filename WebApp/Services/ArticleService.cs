using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Services;

public class ArticleService(IArticleRepository repository,
                                IUserIdProvider userIdProvider) : IArticleService
{
    private readonly IArticleRepository _repository = repository;
    private readonly IUserIdProvider _userIdProvider = userIdProvider;

    public async Task<Article?> GetArticle(string articleId)
    {
        string? userId = await _userIdProvider.GetCurrentUserId();
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetArticleAsync(articleId);
    }

    public async Task<List<Article>?> GetUserArticles()
    {
        string? userId = await _userIdProvider.GetCurrentUserId();
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetUserArticlesAsync(userId);
    }

    public Task<Article?> PostArticle(Article article)
    {
        throw new NotImplementedException();
    }

    public Task<Article?> PatchArticle(Article article)
    {
        throw new NotImplementedException();
    }
}