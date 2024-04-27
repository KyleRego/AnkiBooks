using System.Security.Claims;
using System.Security.Principal;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.WebApp.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace AnkiBooks.WebApp.Services;

public class BookService(  IBookRepository repository,
                                        AuthenticationStateProvider authStateProvider) : IBookService
{
    private readonly IBookRepository _repository = repository;
    private readonly AuthenticationStateProvider _authStateProvider = authStateProvider;

    // Duplicated in ServerUserArticleService, following WET
    private async Task<string?> GetCurrentUserId()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync())
                    .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<List<Book>?> GetPublicBooks()
    {
        return await _repository.GetPublicBooksAsync();
    }
}