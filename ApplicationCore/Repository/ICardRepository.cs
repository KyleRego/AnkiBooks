using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface ICardRepository
{
    Task<Card?> GetCard(string cardId);

    Task<int> GetSuccessfulRepetitionsStreak(Card card);

    Task<Card> UpdateCardAsync(Card card); 
}