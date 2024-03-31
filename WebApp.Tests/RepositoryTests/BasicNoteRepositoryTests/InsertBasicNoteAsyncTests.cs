using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class InsertBasicNoteAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
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
        
            await basicNoteRepository.InsertBasicNoteAsync(basicNote);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = article.ElementsCount() + 1,
                ArticleId = article.Id
            };

            await basicNoteRepository.InsertBasicNoteAsync(basicNote);
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

        await basicNoteRepository.InsertBasicNoteAsync(basicNote);

        article = dbContext.Articles.Include(a => a.BasicNotes)
                                    .Include(a => a.ClozeNotes)
                                    .First(a => a.Id == article.Id);
        BasicNote updatedBasicNote = article.BasicNotes.First(bn => bn.OrdinalPosition == 0);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.Equal(11, article.ElementsCount());
        Assert.True(article.ElementOrdinalPositionsAreCorrect());
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

        await basicNoteRepository.InsertBasicNoteAsync(basicNote);

        article = dbContext.Articles.Include(a => a.BasicNotes)
                                    .Include(a => a.ClozeNotes)
                                    .First(a => a.Id == article.Id);
        BasicNote updatedBasicNote = article.BasicNotes.First(bn => bn.OrdinalPosition == 10);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.Equal(11, article.ElementsCount());
        Assert.True(article.ElementOrdinalPositionsAreCorrect());
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

        await basicNoteRepository.InsertBasicNoteAsync(basicNote);

        article = dbContext.Articles.Include(a => a.BasicNotes)
                                    .Include(a => a.ClozeNotes)
                                    .First(a => a.Id == article.Id);
        BasicNote updatedBasicNote = article.BasicNotes.First(bn => bn.OrdinalPosition == 3);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.Equal(11, article.ElementsCount());
        Assert.True(article.ElementOrdinalPositionsAreCorrect());
    }
}