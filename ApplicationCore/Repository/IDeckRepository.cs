using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IDeckRepository : IOrderedElementRepository<Deck>
{
    Task<List<Deck>> GetDecksAsync();
    Task<Deck?> GetDeckAsync(string mdContentId);
    Task<bool> DeckExists(string mdContentId);
}
