using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class BasicNoteRepository(ApplicationDbContext dbContext)
                            : ArticleElementRepositoryBase(dbContext), IBasicNoteRepository
{
    public async Task<List<BasicNote>> GetBasicNotesAsync()
    {
        return await _dbContext.BasicNotes.ToListAsync();
    }

    public async Task<BasicNote?> GetBasicNoteAsync(string basicNoteId)
    {
        return await _dbContext.BasicNotes.FirstOrDefaultAsync(bn => bn.Id == basicNoteId);
    }

    public async Task<bool> BasicNoteExists(string BasicNoteId)
    {
        return await _dbContext.BasicNotes.AnyAsync(bn => bn.Id == BasicNoteId);
    }
}