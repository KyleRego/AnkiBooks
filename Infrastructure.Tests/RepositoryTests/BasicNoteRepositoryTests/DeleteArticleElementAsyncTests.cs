using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class DeleteArticleElementAsyncTests
{
    [Fact]
    public async Task BasicNoteInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        BasicNote noteToDelete = article.BasicNotes.First(bn => bn.OrdinalPosition == 4);

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.DeleteArticleElementAsync(noteToDelete);

        Assert.Null(dbContext.BasicNotes.FirstOrDefault(bn => bn.Id == noteToDelete.Id));
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 9));
    }
}