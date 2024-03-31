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
        int deletedOrdinalPosition = basicNote.OrdinalPosition;

        _dbContext.BasicNotes.Remove(basicNote);

        Article article = await _dbContext.Articles
            .Include(a => a.BasicNotes.Where(bn => bn.OrdinalPosition > deletedOrdinalPosition && bn.Id != basicNote.Id))
            .Include(a => a.ClozeNotes.Where(cn => cn.OrdinalPosition > deletedOrdinalPosition))
            .FirstAsync(a => a.Id == basicNote.ArticleId);

        foreach (BasicNote bn in article.BasicNotes.Where(bn => bn.OrdinalPosition > deletedOrdinalPosition))
        {
            bn.OrdinalPosition -= 1;
        }

        foreach (ClozeNote cn in article.ClozeNotes.Where(cn => cn.OrdinalPosition > deletedOrdinalPosition))
        {
            cn.OrdinalPosition -= 1;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<BasicNote> UpdateBasicNoteAsync(BasicNote basicNote)
    {
        int originalOrdinalPosition = _dbContext.BasicNotes.AsNoTracking().Single(bn => bn.Id == basicNote.Id).OrdinalPosition;
        int newOrdinalPosition = basicNote.OrdinalPosition;

        if (newOrdinalPosition == originalOrdinalPosition)
        {
            _dbContext.Entry(basicNote).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return basicNote;
        }
        else
        {
            Article article = await _dbContext.Articles
                .Include(a => a.BasicNotes)
                .Include(a => a.ClozeNotes)
                .FirstAsync(a => a.Id == basicNote.ArticleId);

            BasicNote trackedBasicNote = article.BasicNotes.First(bn => bn.Id == basicNote.Id);
            trackedBasicNote.Front = basicNote.Front;
            trackedBasicNote.Back = basicNote.Back;

            if (newOrdinalPosition >= article.ElementsCount() || basicNote.OrdinalPosition < 0)
            {
                // Desired ordinal position is not valid so only update front and back
                await _dbContext.SaveChangesAsync();
                return trackedBasicNote;
            }
            else
            {
                trackedBasicNote.OrdinalPosition = article.ElementsCount();
                await _dbContext.SaveChangesAsync();

                if (newOrdinalPosition > originalOrdinalPosition)
                {
                    List<BasicNote> basicNotesToShiftDown = article.BasicNotes.Where(
                        bn => bn.OrdinalPosition > originalOrdinalPosition && bn.OrdinalPosition <= newOrdinalPosition && bn != trackedBasicNote
                    ).ToList();
                    List<ClozeNote> clozeNotesToShiftDown = article.ClozeNotes.Where(
                        cn => cn.OrdinalPosition > originalOrdinalPosition && cn.OrdinalPosition <= newOrdinalPosition
                    ).ToList();

                    foreach (BasicNote bnToShift in basicNotesToShiftDown)
                    {
                        bnToShift.OrdinalPosition -= 1;
                    }
                    foreach (ClozeNote cnToShift in clozeNotesToShiftDown)
                    {
                        cnToShift.OrdinalPosition -= 1;
                    }
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    List<BasicNote> basicNotesToShiftUp = article.BasicNotes.Where(
                        bn => bn.OrdinalPosition >= newOrdinalPosition && bn.OrdinalPosition < originalOrdinalPosition && bn != trackedBasicNote
                    ).ToList();
                    List<ClozeNote> clozeNotesToShiftUp = article.ClozeNotes.Where(
                        cn => cn.OrdinalPosition >= newOrdinalPosition && cn.OrdinalPosition < originalOrdinalPosition
                    ).ToList();

                    foreach (BasicNote bnToShift in basicNotesToShiftUp)
                    {
                        bnToShift.OrdinalPosition += 1;
                    }
                    foreach (ClozeNote cnToShift in clozeNotesToShiftUp)
                    {
                        cnToShift.OrdinalPosition += 1;
                    }
                    await _dbContext.SaveChangesAsync();
                }

                trackedBasicNote.OrdinalPosition = newOrdinalPosition;
                await _dbContext.SaveChangesAsync();

                return trackedBasicNote;
            }
        }
    }

    public async Task<bool> BasicNoteExists(string BasicNoteId)
    {
        return await _dbContext.BasicNotes.AnyAsync(bn => bn.Id == BasicNoteId);
    }
}