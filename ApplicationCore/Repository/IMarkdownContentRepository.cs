using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IMarkdownContentRepository : IArticleElementRepository<MarkdownContent>
{
    Task<List<MarkdownContent>> GetMarkdownContentsAsync();

    Task<MarkdownContent?> GetMarkdownContentAsync(string mdContentId);

    Task<bool> MarkdownContentExists(string mdContentId);
}
