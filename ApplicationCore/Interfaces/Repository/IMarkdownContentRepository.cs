using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IMarkdownContentRepository : IOrderedElementRepository<MarkdownContent>
{
    Task<List<MarkdownContent>> GetMarkdownContentsAsync();
    Task<MarkdownContent?> GetMarkdownContentAsync(string mdContentId);
    Task<bool> MarkdownContentExists(string mdContentId);
}
