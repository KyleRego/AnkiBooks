using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface IBookService
{
    public Task<List<Book>?> GetPublicBooks();
}