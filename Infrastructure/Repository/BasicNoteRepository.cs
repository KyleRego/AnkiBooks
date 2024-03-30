using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class BasicNoteRepository(ApplicationDbContext dbContext) : IBasicNoteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<BasicNote>> GetBasicNotesAsync()
    {
        return await _dbContext.BasicNotes.ToListAsync();
    }

    public async Task<BasicNote?> GetBasicNoteAsync(string basicNoteId)
    {
        return await _dbContext.BasicNotes.FirstOrDefaultAsync(bn => bn.Id == basicNoteId);
    }

    public async Task<BasicNote> InsertBasicNoteAsync(BasicNote basicNote)
    {
        Article article = await _dbContext.Articles
                .Include(a => a.BasicNotes)
                .Include(a => a.ClozeNotes)
                .FirstAsync(a => a.Id == basicNote.ArticleId);

        if (basicNote.OrdinalPosition > article.ElementsCount() || basicNote.OrdinalPosition < 0)
        {
            basicNote.OrdinalPosition = article.ElementsCount();
        }
        else
        {
            List<BasicNote> basicNotesToShift = article.BasicNotes.Where(bn => bn.OrdinalPosition >= basicNote.OrdinalPosition).ToList();
            List<ClozeNote> clozeNotesToShift = article.ClozeNotes.Where(cn => cn.OrdinalPosition >= basicNote.OrdinalPosition).ToList();

            foreach (BasicNote bnToShift in basicNotesToShift)
            {
                bnToShift.OrdinalPosition += 1;
            }
            foreach (ClozeNote cnToShift in clozeNotesToShift)
            {
                cnToShift.OrdinalPosition += 1;
            }
        }
        
        article.BasicNotes.Add(basicNote);
    
        await _dbContext.SaveChangesAsync();

        return basicNote;
    }

    public async Task DeleteBasicNoteAsync(BasicNote basicNote)
    {
        // TODO: This needs to handle the shifting of other notes
        _dbContext.BasicNotes.Remove(basicNote);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BasicNote> UpdateBasicNoteAsync(BasicNote basicNote)
    {
        int originalOrdinalPosition = _dbContext.BasicNotes.AsNoTracking().Single(bn => bn.Id == basicNote.Id).OrdinalPosition;

        if (basicNote.OrdinalPosition != originalOrdinalPosition)
        {
            // TODO
        }

        _dbContext.Entry(basicNote).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return basicNote;
    }

    public async Task<bool> BasicNoteExists(string BasicNoteId)
    {
        return await _dbContext.BasicNotes.AnyAsync(bn => bn.Id == BasicNoteId);
    }   
}