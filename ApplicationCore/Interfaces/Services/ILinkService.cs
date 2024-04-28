using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces.Services;

public interface ILinkService
{
    public Task<List<Link>?> GetLinks();
    public Task<Link?> PostLink(Link link);
}