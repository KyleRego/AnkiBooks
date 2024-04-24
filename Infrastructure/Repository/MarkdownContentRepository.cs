using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class MarkdownContentRepository(ApplicationDbContext dbContext)
                    : OrderedElementRepositoryBase<MarkdownContent>(dbContext), IMarkdownContentRepository
{
    protected override List<IOrdinalChild> GetAllOrdinalChildren(MarkdownContent mdContent)
    {
        return _dbContext.ArticleElements.Where(
            el => el.ArticleId == mdContent.ArticleId
        ).Cast<IOrdinalChild>().ToList();
    }

    protected override void AddElementToDbContext(MarkdownContent element)
    {
        _dbContext.MarkdownContents.Add(element);
    }

    protected override void RemoveElementFromDbContext(MarkdownContent element)
    {
        _dbContext.MarkdownContents.Remove(element);
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