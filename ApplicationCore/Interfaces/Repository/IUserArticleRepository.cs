using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IUserArticleRepository
{
    Task<List<Article>> GetArticlesAsync(string userId);
    Task<Article?> GetArticleAsync(string userId, string articleId);
    Task<Article> InsertArticleAsync(string userId, Article article);
    Task DeleteArticleAsync(Article article);
    Task<Article> UpdateArticleAsync(Article article);
    Task<bool> ArticleExists(string userId, string articleId);
}
