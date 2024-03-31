using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class UpdateBasicNoteAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task BasicNoteIsUpdated()
    {
        async Task<(string basicNoteId, string articleId)> SetupTest()
        {
            using IServiceScope scope = _factory.Services.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Article article = new("Test article");
            BasicNote basicNote = new() { Front = "Hello", Back = "World", OrdinalPosition = 0 };
            article.BasicNotes.Add(basicNote);
            dbContext.Articles.Add(article);
            await dbContext.SaveChangesAsync();

            return (basicNote.Id, article.Id);
        }
        (string basicNoteId, string articleId) = await SetupTest();

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new()
        {
            Id = basicNoteId,
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 0,
            ArticleId = articleId
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNoteId);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
    }

    [Fact]
    public async Task InvalidBasicNoteOrdinalPositionsThrowAnException()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 2);       

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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
        
            await basicNoteRepository.UpdateBasicNoteAsync(basicNote);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Id = noteToUpdate.Id,
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = article.ElementsCount(),
                ArticleId = article.Id
            };

            await basicNoteRepository.UpdateBasicNoteAsync(basicNote);
        });
    }

    [Fact]
    public async Task BasicNoteIsShiftedToHigherOrdinalPosition()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 2);

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new()
        {
            Id = noteToUpdate.Id,
            Front = "World33",
            Back = "Hello33",
            OrdinalPosition = 5,
            ArticleId = article.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World33", updatedBasicNote.Front);
        Assert.Equal("Hello33", updatedBasicNote.Back);
        Assert.Equal(5, updatedBasicNote.OrdinalPosition);
        article = dbContext.Articles.Include(a => a.BasicNotes)
                                    .Include(a => a.ClozeNotes)
                                    .First(a => a.Id == article.Id);
        Assert.Equal(10, article.ElementsCount());
        Assert.True(article.ElementOrdinalPositionsAreCorrect());
    }

    [Fact]
    public async Task BasicNoteIsShiftedToLowerOrdinalPosition()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 4);

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new()
        {
            Id = noteToUpdate.Id,
            Front = "World334",
            Back = "Hello334",
            OrdinalPosition = 1,
            ArticleId = article.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World334", updatedBasicNote.Front);
        Assert.Equal("Hello334", updatedBasicNote.Back);
        Assert.Equal(1, updatedBasicNote.OrdinalPosition);
        article = dbContext.Articles.Include(a => a.BasicNotes)
                                    .Include(a => a.ClozeNotes)
                                    .First(a => a.Id == article.Id);
        Assert.Equal(10, article.ElementsCount());
        Assert.True(article.ElementOrdinalPositionsAreCorrect());
    }

    [Fact]
    public async Task FirstElementIsMovedToLastPosition()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
        BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 0);

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new()
        {
            Id = noteToUpdate.Id,
            Front = "4321",
            Back = "1234",
            OrdinalPosition = 9,
            ArticleId = article.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("4321", updatedBasicNote.Front);
        Assert.Equal("1234", updatedBasicNote.Back);
        Assert.Equal(9, updatedBasicNote.OrdinalPosition);
        article = dbContext.Articles.Include(a => a.BasicNotes)
                                    .Include(a => a.ClozeNotes)
                                    .First(a => a.Id == article.Id);
        Assert.Equal(10, article.ElementsCount());
        Assert.True(article.ElementOrdinalPositionsAreCorrect());
    }
}