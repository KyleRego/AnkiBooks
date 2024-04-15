using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class BasicNoteRepository(ApplicationDbContext dbContext)
                            : NoteRepositoryBase<BasicNote>(dbContext), IBasicNoteRepository
{
    protected override void AddElementToDbContext(BasicNote element)
    {
        _dbContext.BasicNotes.Add(element);
    }

    protected override void RemoveElementFromDbContext(BasicNote element)
    {
        _dbContext.BasicNotes.Remove(element);
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