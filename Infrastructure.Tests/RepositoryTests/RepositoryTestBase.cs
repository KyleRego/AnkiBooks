using AnkiBooks.ApplicationCore.Entities;
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

        ApplicationDbContext dbContext = new(options);
        dbContext.Database.EnsureCreated();

        return dbContext;
    }

    protected static bool ValidateArticleOrdinalPositions(ApplicationDbContext dbContext, Article article, int expectedElementsCount)
    {
        List<ArticleElement> elements = dbContext.ArticleElements.Where(
            e => e.ArticleId == article.Id
        ).OrderBy(e => e.OrdinalPosition).ToList();

        if (elements.Count != expectedElementsCount)
        {
            return false;
        }

        List<int> ordinalPositions = [];

        foreach (ArticleElement element in elements)
        {
            ordinalPositions.Add(element.OrdinalPosition);
        }

        for (int i = 0; i < expectedElementsCount; i++ )
        {
            if (ordinalPositions[i] != i)
            {
                return false;
            }
        }

        return true;
    }
}