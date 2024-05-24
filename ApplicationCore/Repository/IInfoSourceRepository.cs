using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IInfoSourceRepository
{
    public Task<List<InfoSource>> GetInfoSourcesAsync(int pageNumber);
}