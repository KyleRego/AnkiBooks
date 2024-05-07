using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface ILinkRepository
{
    Task<List<Link>> GetLinksAsync(string userId);
    Task<Link> InsertLinkAsync(Link link);
    Task<Link> UpdateLinkAsync(Link link);
}
