using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IBookRepository
{
    Task<List<Book>> GetPublicBooksAsync();
}
