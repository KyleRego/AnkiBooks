using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
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
        int articleElementsCount = _dbContext.ArticleElements.Count(
            e => e.ArticleId == basicNote.ArticleId
        );

        if (basicNote.OrdinalPosition > articleElementsCount || basicNote.OrdinalPosition < 0)
        {
            throw new OrdinalPositionException();
        }
        else
        {
            List<ArticleElementBase> elementsToShift = _dbContext.ArticleElements.Where(
                e => e.ArticleId == basicNote.ArticleId && e.OrdinalPosition >= basicNote.OrdinalPosition
            ).ToList();

            foreach (ArticleElementBase e in elementsToShift) { e.OrdinalPosition += 1; }
        }

        _dbContext.BasicNotes.Add(basicNote);

        await _dbContext.SaveChangesAsync();

        return basicNote;
    }

    public async Task DeleteBasicNoteAsync(BasicNote basicNote)
    {
        int deletedOrdinalPosition = basicNote.OrdinalPosition;

        _dbContext.BasicNotes.Remove(basicNote);

        List<ArticleElementBase> elementsToShift = _dbContext.ArticleElements.Where(
            e => e.ArticleId == basicNote.ArticleId && e.OrdinalPosition > deletedOrdinalPosition
        ).ToList();

        foreach (ArticleElementBase e in elementsToShift) { e.OrdinalPosition -= 1; }

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
            int articleElementsCount = _dbContext.ArticleElements.AsNoTracking().Count(
                e => e.ArticleId == basicNote.ArticleId
            );

            if (newOrdinalPosition >= articleElementsCount || basicNote.OrdinalPosition < 0)
            {
                throw new OrdinalPositionException();
            }
            else
            {
                BasicNote trackedBasicNote = _dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
                trackedBasicNote.Front = basicNote.Front;
                trackedBasicNote.Back = basicNote.Back;
                trackedBasicNote.OrdinalPosition = articleElementsCount;

                if (newOrdinalPosition > originalOrdinalPosition)
                {
                    List<ArticleElementBase> elementsToShiftDown = _dbContext.ArticleElements.Where(
                        e => e.ArticleId == basicNote.ArticleId 
                        && e.OrdinalPosition > originalOrdinalPosition
                        && e.OrdinalPosition <= newOrdinalPosition
                        && e.Id != basicNote.Id
                    ).ToList();

                    foreach (ArticleElementBase e in elementsToShiftDown) { e.OrdinalPosition -= 1; }

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    List<ArticleElementBase> elementsToShiftUp = _dbContext.ArticleElements.Where(
                        e => e.ArticleId == basicNote.ArticleId
                        && e.OrdinalPosition < originalOrdinalPosition
                        && e.OrdinalPosition >= newOrdinalPosition
                        && e.Id != basicNote.Id
                    ).ToList();

                    foreach (ArticleElementBase e in elementsToShiftUp) { e.OrdinalPosition += 1; }

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