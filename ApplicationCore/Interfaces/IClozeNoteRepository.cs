using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IClozeNoteRepository : IArticleElementRepository<ArticleNoteBase>
{
    Task<List<ClozeNote>> GetClozeNotesAsync();
    Task<ClozeNote?> GetClozeNoteAsync(string ClozeNoteId);
    Task<bool> ClozeNoteExists(string id);
}
