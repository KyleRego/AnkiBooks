using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces.Services;

public interface IUserArticleService
{
    public Task<Article?> GetUserArticle(string articleId, string? userId);
    public Task<List<Article>?> GetUserArticles(string? userId);
    public Task<Article?> PostUserArticle(Article articleData);
}