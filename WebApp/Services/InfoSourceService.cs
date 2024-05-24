using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Services;

public class InfoSourceService(IInfoSourceRepository repository) : IInfoSourceService
{
    private readonly IInfoSourceRepository _repository = repository;

    public async Task<List<InfoSource>?> GetInfoSources(int pageNumber)
    {
        if (pageNumber < 1) pageNumber = 1;

        return await _repository.GetInfoSourcesAsync(pageNumber);
    }
}