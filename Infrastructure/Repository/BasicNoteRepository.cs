using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class BasicNoteRepository(ApplicationDbContext dbContext)
                            : OrderedElementRepositoryBase<BasicNote>(dbContext), IBasicNoteRepository
{
    protected override List<IOrdinalChild> GetAllOrdinalChildren(BasicNote basicNote)
    {
        return _dbContext.Notes.Where(
            n => n.SectionId == basicNote.SectionId
        ).Cast<IOrdinalChild>().ToList();
    }

    protected override void AddElementToDbContext(BasicNote basicNote)
    {
        _dbContext.BasicNotes.Add(basicNote);
    }

    protected override void RemoveElementFromDbContext(BasicNote basicNote)
    {
        _dbContext.BasicNotes.Remove(basicNote);
    }

    public async Task<List<BasicNote>> GetBasicNotesAsync()
    {
        return await _dbContext.BasicNotes.ToListAsync();
    }

    public async Task<BasicNote?> GetBasicNoteAsync(string basicNoteId)
    {
        return await _dbContext.BasicNotes.FirstOrDefaultAsync(bn => bn.Id == basicNoteId);
    }

    public async Task<bool> BasicNoteExists(string basicNoteId)
    {
        return await _dbContext.BasicNotes.AnyAsync(bn => bn.Id == basicNoteId);
    }
}