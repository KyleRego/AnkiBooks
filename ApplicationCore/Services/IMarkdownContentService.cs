using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface IMarkdownContentService
{
    public Task<MarkdownContent?> PostMarkdownContent(MarkdownContent mdContent);

    public Task<MarkdownContent?> PutMarkdownContent(MarkdownContent mdContent);

    public Task DeleteMarkdownContent(string mdContentId);
}