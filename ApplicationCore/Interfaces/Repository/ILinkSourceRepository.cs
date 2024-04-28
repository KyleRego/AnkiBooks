using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IInfoSourceRepository
{
    Task<List<InfoSource>> GetInfoSourcesAsync(string userId);
    Task<LinkSource> InsertLinkSourceAsync(LinkSource linkSource);
}
