using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Services;

public interface IRepetitionService
{
    public Task<Repetition?> PostRepetition(Repetition rep);
}