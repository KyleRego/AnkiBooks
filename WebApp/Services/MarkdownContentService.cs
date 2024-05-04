using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Services;

public class MarkdownContentService(IMarkdownContentRepository repository) : IMarkdownContentService
{
    private readonly IMarkdownContentRepository _repository = repository;

    public async Task DeleteMarkdownContent(string mdContentId)
    {
        MarkdownContent? mdContent = await _repository.GetMarkdownContentAsync(mdContentId);

        if (mdContent == null) return;

        await _repository.DeleteOrderedElementAsync(mdContent);
    }

    public Task<MarkdownContent?> PostMarkdownContent(MarkdownContent mdContent)
    {
        throw new NotImplementedException();
    }

    public Task<MarkdownContent?> PutMarkdownContent(MarkdownContent mdContent)
    {
        throw new NotImplementedException();
    }
}