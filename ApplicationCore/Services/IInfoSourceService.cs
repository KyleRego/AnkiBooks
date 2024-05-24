using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface IInfoSourceService
{
    public Task<List<InfoSource>?> GetInfoSources(int pageNumber);
}