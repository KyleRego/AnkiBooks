using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;

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
    public async Task BasicNotIsNotUpdatedToOrdinalPositionGreaterThanRange()
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
            OrdinalPosition = 1,
            ArticleId = articleId
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNoteId);
        Assert.Equal(0, updatedBasicNote.OrdinalPosition);      
    }

    [Fact]
    public async Task BasicNoteIsUpdatedToHigherOrdinalPosition()
    {
        async Task<(string basicNoteId, string articleId)> SetupTest()
        {
            Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
            BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 2);

            return (noteToUpdate.Id, article.Id);
        }

        (string basicNoteId, string articleId) = await SetupTest();

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new()
        {
            Id = basicNoteId,
            Front = "World33",
            Back = "Hello33",
            OrdinalPosition = 5,
            ArticleId = articleId
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNoteId);
        Assert.Equal("World33", updatedBasicNote.Front);
        Assert.Equal("Hello33", updatedBasicNote.Back);
        Assert.Equal(5, updatedBasicNote.OrdinalPosition);
    }

    [Fact]
    public async Task BasicNoteIsUpdatedToLowerOrdinalPosition()
    {
        async Task<(string basicNoteId, string articleId)> SetupTest()
        {
            Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
            BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 4);

            return (noteToUpdate.Id, article.Id);
        }

        (string basicNoteId, string articleId) = await SetupTest();

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new()
        {
            Id = basicNoteId,
            Front = "World334",
            Back = "Hello334",
            OrdinalPosition = 1,
            ArticleId = articleId
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNoteId);
        Assert.Equal("World334", updatedBasicNote.Front);
        Assert.Equal("Hello334", updatedBasicNote.Back);
        Assert.Equal(1, updatedBasicNote.OrdinalPosition);
    }

    [Fact]
    public async Task FirstElementIsMovedToLastPosition()
    {
        async Task<(string basicNoteId, string articleId)> SetupTest()
        {
            Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
            BasicNote noteToUpdate = article.BasicNotes.First(bn => bn.OrdinalPosition == 0);

            return (noteToUpdate.Id, article.Id);
        }

        (string basicNoteId, string articleId) = await SetupTest();

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new()
        {
            Id = basicNoteId,
            Front = "4321",
            Back = "1234",
            OrdinalPosition = 9,
            ArticleId = articleId
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateBasicNoteAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNoteId);
        Assert.Equal("4321", updatedBasicNote.Front);
        Assert.Equal("1234", updatedBasicNote.Back);
        Assert.Equal(9, updatedBasicNote.OrdinalPosition);
    }
}