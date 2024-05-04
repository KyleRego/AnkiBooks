using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Services;

public class DeckService(IDeckRepository repository) : IDeckService
{
    private readonly IDeckRepository _repository = repository;

    public async Task DeleteDeck(string deckId)
    {
        Deck? deck = await _repository.GetDeckAsync(deckId);

        if (deck == null) return;

        await _repository.DeleteOrderedElementAsync(deck);
    }

    public Task<Deck?> PostDeck(Deck deck)
    {
        throw new NotImplementedException();
    }

    public Task<Deck?> PutDeck(Deck deck)
    {
        throw new NotImplementedException();
    }
}