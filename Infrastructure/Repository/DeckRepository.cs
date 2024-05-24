using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class DeckRepository(ApplicationDbContext dbContext)
                    : ArticleElementRepository<Deck>(dbContext), IDeckRepository
{
    public async Task<List<Deck>> GetDecksAsync()
    {
        return await _dbContext.Decks.ToListAsync();
    }

    public async Task<Deck?> GetDeckAsync(string clozeNoteId)
    {
        return await _dbContext.Decks
                                .Include(d => d.BasicNotes)
                                .Include(d => d.ClozeNotes)
                                .FirstOrDefaultAsync(bn => bn.Id == clozeNoteId);
    }

    public async Task<bool> DeckExists(string clozeNoteId)
    {
        return await _dbContext.Decks.AnyAsync(a => a.Id == clozeNoteId);
    }
}