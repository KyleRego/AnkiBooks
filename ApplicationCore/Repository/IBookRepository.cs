using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IBookRepository
{
    Task<List<Book>> GetPublicBooksAsync();
}
