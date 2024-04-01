using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IBasicNoteRepository : IArticleElementRepository
{
    Task<List<BasicNote>> GetBasicNotesAsync();
    Task<BasicNote?> GetBasicNoteAsync(string BasicNoteId);
    Task<bool> BasicNoteExists(string id);
}
