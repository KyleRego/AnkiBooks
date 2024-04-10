using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class InsertArticleElementAsyncTests
{
    [Fact]
    public async Task ClozeNoteIsInsertedInMiddleOfArticleWithNotes()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();

        ClozeNote clozeNote = new()
        {
            Text = "World2Cloze",
            OrdinalPosition = 6,
            ArticleId = article.Id
        };

        ClozeNoteRepository clozeNoteRepository = new(dbContext);

        await clozeNoteRepository.InsertArticleElementAsync(clozeNote);

        ClozeNote updatedClozeNote = dbContext.ClozeNotes.First(cn => cn.Id == clozeNote.Id);
        Assert.Equal("World2Cloze", updatedClozeNote.Text);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 11));
    }
}