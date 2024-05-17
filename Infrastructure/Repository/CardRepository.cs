using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Enums;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class CardRepository(ApplicationDbContext dbContext) : ICardRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<Card?> GetCard(string cardId)
    {
        return await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId);
    }

    public async Task<int> GetSuccessfulRepetitionsStreak(Card card)
    {
        Repetition? mostRecentFailedRep = await _dbContext.Repetitions
                                            .Where(rep => rep.CardId == card.Id && rep.Grade == Grade.Bad)
                                            .OrderBy(rep => rep.OccurredAt).LastOrDefaultAsync();
        if (mostRecentFailedRep == null)
        {
            return await _dbContext.Repetitions.Where(rep => rep.CardId == card.Id).CountAsync();
        }
        else
        {
            return await _dbContext.Repetitions.Where(
                rep => rep.CardId == card.Id && rep.OccurredAt > mostRecentFailedRep.OccurredAt
            ).CountAsync();
        }
    }

    public async Task<Card> UpdateCardAsync(Card card)
    {
        _dbContext.Entry(card).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return card;
    }
}