using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface ILinkService
{
    public Task<List<Link>?> GetLinks();

    public Task<Link?> PostLink(Link link);

    public Task<Link?> PutLink(Link link);
}