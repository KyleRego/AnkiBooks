using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp;

public class ServerApiService(IUserArticleRepository repository) : INewAnkiBooksApiService
{
    private readonly IUserArticleRepository _repository = repository;

    public async Task<List<Article>?> GetUserArticles(string? userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetArticlesAsync(userId);
    }
}