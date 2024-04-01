using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IArticleElementRepository
{
    Task<ArticleElementBase> InsertArticleElementAsync(ArticleElementBase element);
    Task DeleteArticleElementAsync(ArticleElementBase element);
    Task<ArticleElementBase> UpdateArticleElementAsync(ArticleElementBase element);
}