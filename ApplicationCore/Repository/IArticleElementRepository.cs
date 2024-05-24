using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IArticleElementRepository<T> where T : IArticleElement
{
    Task<T> InsertAsync(T element);

    Task DeleteAsync(T element);

    Task<T> UpdateAsync(T element);
}