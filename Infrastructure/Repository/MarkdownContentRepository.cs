using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class MarkdownContentRepository(ApplicationDbContext dbContext)
            : ArticleContentRepository(dbContext), IMarkdownContentRepository
{
    public async Task<List<MarkdownContent>> GetMarkdownContentsAsync()
    {
        return await _dbContext.MarkdownContents.ToListAsync();
    }

    public async Task<MarkdownContent?> GetMarkdownContentAsync(string markdownContentId)
    {
        return await _dbContext.MarkdownContents.FirstOrDefaultAsync(bn => bn.Id == markdownContentId);
    }

    public async Task<bool> MarkdownContentExists(string markdownContentId)
    {
        return await _dbContext.MarkdownContents.AnyAsync(a => a.Id == markdownContentId);
    }
}