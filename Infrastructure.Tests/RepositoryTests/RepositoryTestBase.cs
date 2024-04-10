using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests;

public abstract class RepositoryTestBase
{
    public static ApplicationDbContext InMemoryDbContext()
    {
        SqliteConnection connection = new("DataSource=:memory:");
        connection.Open();

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        ApplicationDbContext dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}