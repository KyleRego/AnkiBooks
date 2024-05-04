using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IBasicNoteRepository
{
    Task<BasicNote?> GetBasicNoteAsync(string basicNoteId);

    Task<BasicNote> InsertBasicNoteAsync(BasicNote basicNote);

    Task DeleteBasicNoteAsync(BasicNote basicNote);

    Task<BasicNote> UpdateBasicNoteAsync(BasicNote basicNote);

    Task<bool> BasicNoteExists(string id);
}
