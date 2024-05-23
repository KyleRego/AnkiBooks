using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class MarkdownContentRepository(ApplicationDbContext dbContext)
                    : OrderedElementRepositoryBase<MarkdownContent>(dbContext), IMarkdownContentRepository
{
    protected override List<IOrdinalChild> GetAllOrdinalSiblings(MarkdownContent mdContent)
    {
        return _dbContext.ArticleElements.Where(
            el => el.ArticleId == mdContent.ArticleId && el.Id != mdContent.Id
        ).Cast<IOrdinalChild>().ToList();
    }

    protected override int GetOriginalPosition(string elementId)
    {
        return _dbContext.MarkdownContents.AsNoTracking().First(md => md.Id == elementId).OrdinalPosition;
    }

    public async Task<List<MarkdownContent>> GetMarkdownContentsAsync()
    {
        return await _dbContext.MarkdownContents.ToListAsync();
    }

    public async Task<MarkdownContent?> GetMarkdownContentAsync(string clozeNoteId)
    {
        return await _dbContext.MarkdownContents.FirstOrDefaultAsync(bn => bn.Id == clozeNoteId);
    }

    public async Task<bool> MarkdownContentExists(string clozeNoteId)
    {
        return await _dbContext.MarkdownContents.AnyAsync(a => a.Id == clozeNoteId);
    }
}