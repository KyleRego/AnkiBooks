using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces.Services;

public interface IBookService
{
    public Task<List<Book>?> GetPublicBooks();
}