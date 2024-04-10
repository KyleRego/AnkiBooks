using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class UpdateArticleElementAsyncTests
{
    [Fact]
    public async Task BasicNoteIsUpdated()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        Article article = await dbContext.CreateArticleWithOneBasicNote();

        BasicNote basicNote = article.BasicNotes.First();
        
        basicNote.Front = "World2";
        basicNote.Back = "Hello2";

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateArticleElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
    }

    [Fact]
    public async Task InvalidBasicNoteOrdinalPositionsThrowAnException()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 2);  

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Id = noteToUpdate.Id,
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = -1,
                ArticleId = article.Id
            };
        
            await basicNoteRepository.UpdateArticleElementAsync(basicNote);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Id = noteToUpdate.Id,
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = 10,
                ArticleId = article.Id
            };

            await basicNoteRepository.UpdateArticleElementAsync(basicNote);
        });
    }

    [Fact]
    public async Task BasicNoteIsShiftedToHigherOrdinalPosition()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 2);

        BasicNote basicNote = new()
        {
            Id = noteToUpdate.Id,
            Front = "World33",
            Back = "Hello33",
            OrdinalPosition = 5,
            ArticleId = article.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateArticleElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World33", updatedBasicNote.Front);
        Assert.Equal("Hello33", updatedBasicNote.Back);
        Assert.Equal(5, updatedBasicNote.OrdinalPosition);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 10));
    }

    [Fact]
    public async Task BasicNoteIsShiftedToLowerOrdinalPosition()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 4);

        BasicNote basicNote = new()
        {
            Id = noteToUpdate.Id,
            Front = "World334",
            Back = "Hello334",
            OrdinalPosition = 1,
            ArticleId = article.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateArticleElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World334", updatedBasicNote.Front);
        Assert.Equal("Hello334", updatedBasicNote.Back);
        Assert.Equal(1, updatedBasicNote.OrdinalPosition);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 10));
    }

    [Fact]
    public async Task FirstElementIsMovedToLastPosition()
    {
        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        using var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 0);

        BasicNote basicNote = new()
        {
            Id = noteToUpdate.Id,
            Front = "4321",
            Back = "1234",
            OrdinalPosition = 9,
            ArticleId = article.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateArticleElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("4321", updatedBasicNote.Front);
        Assert.Equal("1234", updatedBasicNote.Back);
        Assert.Equal(9, updatedBasicNote.OrdinalPosition);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 10));
    }
}