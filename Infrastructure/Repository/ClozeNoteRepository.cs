using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class ClozeNoteRepository(ApplicationDbContext dbContext)
                    : OrderedElementRepositoryBase<ClozeNote>(dbContext), IClozeNoteRepository
{
    protected override List<IOrdinalChild> GetAllOrdinalChildren(ClozeNote clozeNote)
    {
        return _dbContext.ArticleElements.Where(
            el => el.ArticleId == clozeNote.ArticleId
        ).Cast<IOrdinalChild>().ToList();
    }

    protected override void AddElementToDbContext(ClozeNote element)
    {
        _dbContext.ClozeNotes.Add(element);
    }

    protected override void RemoveElementFromDbContext(ClozeNote element)
    {
        _dbContext.ClozeNotes.Remove(element);
    }

    public async Task<List<ClozeNote>> GetClozeNotesAsync()
    {
        return await _dbContext.ClozeNotes.ToListAsync();
    }

    public async Task<ClozeNote?> GetClozeNoteAsync(string clozeNoteId)
    {
        return await _dbContext.ClozeNotes.FirstOrDefaultAsync(bn => bn.Id == clozeNoteId);
    }

    public async Task<bool> ClozeNoteExists(string clozeNoteId)
    {
        return await _dbContext.ClozeNotes.AnyAsync(a => a.Id == clozeNoteId);
    }
}