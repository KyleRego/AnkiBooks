using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class LinkRepository(ApplicationDbContext dbContext) : ILinkRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Link>> GetLinksAsync(string userId)
    {
        return await _dbContext.Links
                            .Where(ls => ls.UserId == userId)
                            .ToListAsync();
    }

    public async Task<Link> InsertLinkAsync(Link link)
    {
        _dbContext.Links.Add(link);
        await _dbContext.SaveChangesAsync();
        return link;
    }
}