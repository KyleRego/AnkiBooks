using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IClozeNoteRepository : IOrderedElementRepository<ClozeNote>
{
    Task<List<ClozeNote>> GetClozeNotesAsync();
    Task<ClozeNote?> GetClozeNoteAsync(string ClozeNoteId);
    Task<bool> ClozeNoteExists(string id);
}
