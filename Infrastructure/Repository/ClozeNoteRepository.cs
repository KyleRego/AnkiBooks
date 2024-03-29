using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class ClozeNoteRepository(ApplicationDbContext dbContext) : IClozeNoteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<ClozeNote>> GetClozeNotesAsync()
    {
        return await _dbContext.ClozeNotes.ToListAsync();
    }

    public async Task<ClozeNote?> GetClozeNoteAsync(string clozeNoteId)
    {
        return await _dbContext.ClozeNotes.FirstOrDefaultAsync(bn => bn.Id == clozeNoteId);
    }

    public async Task<ClozeNote> InsertClozeNoteAsync(ClozeNote clozeNote)
    {
        Article article = await _dbContext.Articles
                .Include(a => a.BasicNotes)
                .Include(a => a.ClozeNotes)
                .FirstAsync(a => a.Id == clozeNote.ArticleId);

        List<BasicNote> basicNotesToShift = article.BasicNotes.Where(bn => bn.OrdinalPosition >= clozeNote.OrdinalPosition).ToList();
        List<ClozeNote> clozeNotesToShift = article.ClozeNotes.Where(cn => cn.OrdinalPosition >= clozeNote.OrdinalPosition).ToList();
        foreach (BasicNote bnToShift in basicNotesToShift)
        {
            bnToShift.OrdinalPosition += 1;
        }
        foreach (ClozeNote cnToShift in clozeNotesToShift)
        {
            cnToShift.OrdinalPosition += 1;
        }
        article.ClozeNotes.Add(clozeNote);

        await _dbContext.SaveChangesAsync();

        return clozeNote;
    }

    public async Task DeleteClozeNoteAsync(ClozeNote clozeNote)
    {
        // TODO: This needs to handle the shifting of other notes
        _dbContext.ClozeNotes.Remove(clozeNote);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ClozeNote> UpdateClozeNoteAsync(ClozeNote clozeNote)
    {
        // TODO: This needs to handle the shifting of other notes
        _dbContext.Entry(clozeNote).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return clozeNote;
    }

    public async Task<bool> ClozeNoteExists(string clozeNoteId)
    {
        return await _dbContext.ClozeNotes.AnyAsync(a => a.Id == clozeNoteId);
    }
}