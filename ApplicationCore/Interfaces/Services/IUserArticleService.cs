using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces.Services;

public interface IUserArticleService
{
    public Task<Article?> GetUserArticle(string articleId);
    public Task<List<Article>?> GetUserArticles();
    public Task<Article?> PostUserArticle(Article articleData);
}