using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class ClozeNoteRepository(ApplicationDbContext dbContext) : IClozeNoteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<ClozeNote?> GetClozeNoteAsync(string clozeNoteId)
    {
        return await _dbContext.ClozeNotes.FirstOrDefaultAsync(cn => cn.Id == clozeNoteId);
    }

    public async Task<ClozeNote> InsertClozeNoteAsync(ClozeNote clozeNote)
    {
        _dbContext.ClozeNotes.Add(clozeNote);
        await _dbContext.SaveChangesAsync();
        return clozeNote;
    }

    public async Task DeleteClozeNoteAsync(ClozeNote clozeNote)
    {
        _dbContext.ClozeNotes.Remove(clozeNote);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ClozeNote> UpdateClozeNoteAsync(ClozeNote clozeNote)
    {
        _dbContext.Entry(clozeNote).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return clozeNote;
    }

    public async Task<bool> ClozeNoteExists(string clozeNoteId)
    {
        return await _dbContext.ClozeNotes.AnyAsync(a => a.Id == clozeNoteId);
    }
}