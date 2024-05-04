using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Services;

public class BookService(   IBookRepository repository,
                            IUserIdProvider userIdProvider) : IBookService
{
    private readonly IBookRepository _repository = repository;
    private readonly IUserIdProvider _userIdProvider = userIdProvider;

    public async Task<List<Book>?> GetPublicBooks()
    {
        return await _repository.GetPublicBooksAsync();
    }
}