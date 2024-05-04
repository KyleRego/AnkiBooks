using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface ICardService
{
    public Task<Card> PostCard(Card card);

    public Task<Card> PutCard(Card card);

    public Task DeleteCard(string cardId);
}