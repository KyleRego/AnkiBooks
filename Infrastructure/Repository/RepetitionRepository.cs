using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.Infrastructure.Repository;

public class RepetitionRepository(ApplicationDbContext context) : IRepetitionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Repetition> InsertRepetition(Repetition rep)
    {
        _context.Repetitions.Add(rep);
        await _context.SaveChangesAsync();
        return rep;
    }
}