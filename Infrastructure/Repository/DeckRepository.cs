using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class DeckRepository(ApplicationDbContext dbContext)
                    : OrderedElementRepositoryBase<Deck>(dbContext), IDeckRepository
{
    protected override List<IOrdinalChild> GetAllOrdinalChildren(Deck mdContent)
    {
        return _dbContext.ArticleElements.Where(
            el => el.ArticleId == mdContent.ArticleId
        ).Cast<IOrdinalChild>().ToList();
    }

    protected override void AddElementToDbContext(Deck element)
    {
        _dbContext.Decks.Add(element);
    }

    protected override void RemoveElementFromDbContext(Deck element)
    {
        _dbContext.Decks.Remove(element);
    }

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