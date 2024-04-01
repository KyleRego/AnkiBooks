using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.WebApp.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class InsertArticleElementAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task InvalidBasicNoteOrdinalPositionsThrowAnException()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);  

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = -1,
                ArticleId = article.Id
            };
        
            await basicNoteRepository.InsertArticleElementAsync(basicNote);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = 11,
                ArticleId = article.Id
            };

            await basicNoteRepository.InsertArticleElementAsync(basicNote);
        });
    }

    [Fact]
    public async Task BasicNoteIsInsertedAtBeginningOfArticleWithNotes()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);

        BasicNote basicNote = new()
        {
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 0,
            ArticleId = article.Id
        };

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.InsertArticleElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 11));
    }

    [Fact]
    public async Task BasicNoteIsInsertedAtEndOfArticleWithNotes()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);

        BasicNote basicNote = new()
        {
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 10,
            ArticleId = article.Id
        };

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.InsertArticleElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 11));
    }

    [Fact]
    public async Task BasicNoteIsInsertedInMiddleOfArticleWithNotes()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);

        BasicNote basicNote = new()
        {
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 3,
            ArticleId = article.Id
        };

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.InsertArticleElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 11));
    }
}