using Microsoft.EntityFrameworkCore;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.Infrastructure.Repository;

public class InfoSourceRepository(ApplicationDbContext dbContext) : IInfoSourceRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    private readonly int sourcesPerPage = 25;

    public async Task<List<InfoSource>> GetInfoSourcesAsync(int pageNumber)
    {
        // TODO: Keyset pagination is recommended over this
        int amountToSkip = (pageNumber - 1) * sourcesPerPage;
        return await _dbContext.InfoSources.OrderBy(source => source.Id)
                                            .Skip(amountToSkip)
                                            .Take(sourcesPerPage)
                                            .ToListAsync();
    }
}