using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface IArticleElementService
{
    public Task<ArticleElement?> PostArticleElement(ArticleElement artElement);

    public Task<ArticleElement?> PutArticleElement(ArticleElement artElement);

    public Task DeleteArticleElement(ArticleElement articleElement);
}