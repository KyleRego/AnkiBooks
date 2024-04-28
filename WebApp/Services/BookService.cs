using System.Security.Claims;
using System.Security.Principal;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.WebApp.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;

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