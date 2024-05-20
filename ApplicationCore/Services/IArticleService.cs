using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface IArticleService
{
    public Task<Article?> GetArticle(string articleId);

    public Task<List<Article>?> GetUserArticles();

    public Task<Article?> PostArticle(Article article);

    public Task<Article?> PatchArticle(Article article);

    public Task DeleteArticle(string articleId);
}