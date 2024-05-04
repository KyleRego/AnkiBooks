

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class BookRepository(ApplicationDbContext dbContext) : IBookRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Book>> GetPublicBooksAsync()
    {
        return await _dbContext.Books.Where(b => b.PublicViewable == true).ToListAsync();
    }
}