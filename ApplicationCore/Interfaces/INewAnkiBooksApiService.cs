using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface INewAnkiBooksApiService
{
    public Task<List<Article>?> GetUserArticles(string? userId);
}