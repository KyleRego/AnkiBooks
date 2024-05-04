using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IArticleRepository
{
    Task<List<Article>> GetArticlesAsync();
    Task<Article?> GetArticleAsync(string articleId);
    Task<Article> InsertArticleAsync(Article article);
    Task DeleteArticleAsync(Article article);
    Task<Article> UpdateArticleAsync(Article article);
    Task<bool> ArticleExists(string id);
}
