using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces.Services;

public interface IInfoSourceService
{
    public Task<List<InfoSource>?> GetInfoSources();
    public Task<LinkSource?> PostLinkSource(LinkSource linkSource);
}