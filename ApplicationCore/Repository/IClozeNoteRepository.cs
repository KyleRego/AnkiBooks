using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IClozeNoteRepository
{
    Task<ClozeNote?> GetClozeNoteAsync(string clozeNoteId);

    Task<ClozeNote> InsertClozeNoteAsync(ClozeNote clozeNote);

    Task DeleteClozeNoteAsync(ClozeNote clozeNote);

    Task<ClozeNote> UpdateClozeNoteAsync(ClozeNote clozeNote); 

    Task<bool> ClozeNoteExists(string id);
}
