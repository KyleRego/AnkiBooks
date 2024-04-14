using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IArticleElementRepository<T>
{
    Task<T> InsertArticleElementAsync(T element);
    Task DeleteArticleElementAsync(T element);
    Task<T> UpdateArticleElementAsync(T element);
}