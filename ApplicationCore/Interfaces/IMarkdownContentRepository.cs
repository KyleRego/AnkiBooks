using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IMarkdownContentRepository : IArticleElementRepository
{
    Task<List<MarkdownContent>> GetMarkdownContentsAsync();
    Task<MarkdownContent?> GetMarkdownContentAsync(string markdownContentId);
    Task<bool> MarkdownContentExists(string id);
}
