using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class InsertBasicNoteAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task BasicNoteIsInsertedInMiddleOfArticleWithNotes()
    {
        async Task<string> SetupTest()
        {
            using IServiceScope scope = _factory.Services.CreateScope();
            ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Article article = new("Test article");
            List<BasicNote> existingBasicNotes =
            [
                new() { Front = "Front", Back = "Back", OrdinalPosition = 0 },
                new() { Front = "Front", Back = "Back", OrdinalPosition = 2 },
                new() { Front = "Front", Back = "Back", OrdinalPosition = 4 }
            ];
            List<ClozeNote> existingClozeNotes =
            [
                new() { Text = "a", OrdinalPosition = 1},
                new() { Text = "b", OrdinalPosition = 3}
            ];
            
            article.BasicNotes = existingBasicNotes;
            article.ClozeNotes = existingClozeNotes;
            dbContext.Articles.Add(article);
            await dbContext.SaveChangesAsync();

            return article.Id;
        }
        string articleId = await SetupTest();

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote newBasicNote = new()
        {
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 3,
            ArticleId = articleId
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.InsertBasicNoteAsync(newBasicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.OrdinalPosition == 3);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
    }
}