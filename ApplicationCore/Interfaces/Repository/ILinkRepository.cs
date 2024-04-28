using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface ILinkRepository
{
    Task<List<Link>> GetLinksAsync(string userId);
    Task<Link> InsertLinkAsync(Link link);
}
