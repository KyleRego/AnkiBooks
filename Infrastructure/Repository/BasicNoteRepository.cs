using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class BasicNoteRepository(ApplicationDbContext dbContext) : IBasicNoteRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<bool> BasicNoteExists(string basicNoteId)
    {
        return await _dbContext.BasicNotes.AnyAsync(bn => bn.Id == basicNoteId);
    }

    public async Task<BasicNote?> GetBasicNoteAsync(string basicNoteId)
    {
        return await _dbContext.BasicNotes.FirstOrDefaultAsync(bn => bn.Id == basicNoteId);
    }

    public async Task<BasicNote> InsertBasicNoteAsync(BasicNote basicNote)
    {
        _dbContext.BasicNotes.Add(basicNote);
        await _dbContext.SaveChangesAsync();
        return basicNote;
    }

    public async Task DeleteBasicNoteAsync(BasicNote basicNote)
    {
        _dbContext.BasicNotes.Remove(basicNote);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BasicNote> UpdateBasicNoteAsync(BasicNote basicNote)
    {
        _dbContext.Entry(basicNote).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return basicNote;
    }
}