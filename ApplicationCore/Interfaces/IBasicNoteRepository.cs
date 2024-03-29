using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IBasicNoteRepository
{
    Task<List<BasicNote>> GetBasicNotesAsync();
    Task<BasicNote?> GetBasicNoteAsync(string BasicNoteId);
    Task<BasicNote> InsertBasicNoteAsync(BasicNote BasicNote);
    Task DeleteBasicNoteAsync(BasicNote BasicNote);
    Task<BasicNote> UpdateBasicNoteAsync(BasicNote BasicNote);
    Task<bool> BasicNoteExists(string id);
}
