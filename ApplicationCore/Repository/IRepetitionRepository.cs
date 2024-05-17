using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IRepetitionRepository
{
    Task<Repetition> InsertRepetition(Repetition rep);
}