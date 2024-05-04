using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface IDeckService
{
    public Task<Deck?> PostDeck(Deck mdContent);

    public Task<Deck?> PutDeck(Deck mdContent);

    public Task DeleteDeck(string mdContentId);
}