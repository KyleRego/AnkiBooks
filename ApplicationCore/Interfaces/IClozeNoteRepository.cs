using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IClozeNoteRepository
{
    Task<List<ClozeNote>> GetClozeNotesAsync();
    Task<ClozeNote?> GetClozeNoteAsync(string ClozeNoteId);
    Task<ClozeNote> InsertClozeNoteAsync(ClozeNote ClozeNote);
    Task DeleteClozeNoteAsync(ClozeNote ClozeNote);
    Task<ClozeNote> UpdateClozeNoteAsync(ClozeNote ClozeNote);
    Task<bool> ClozeNoteExists(string id);
}
