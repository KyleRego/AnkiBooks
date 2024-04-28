using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class InfoSourceRepository(ApplicationDbContext dbContext) : IInfoSourceRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<InfoSource>> GetInfoSourcesAsync(string userId)
    {
        return await _dbContext.InfoSources
                            .Where(ls => ls.UserId == userId)
                            .ToListAsync();
    }

    public async Task<LinkSource> InsertLinkSourceAsync(LinkSource linkSource)
    {
        _dbContext.LinkSources.Add(linkSource);
        await _dbContext.SaveChangesAsync();
        return linkSource;
    }
}