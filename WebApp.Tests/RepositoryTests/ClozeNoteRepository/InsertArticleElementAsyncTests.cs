using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.WebApp.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class InsertArticleElementAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task ClozeNoteIsInsertedInMiddleOfArticleWithNotes()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);

        ClozeNote clozeNote = new()
        {
            Text = "World2Cloze",
            OrdinalPosition = 6,
            ArticleId = article.Id
        };

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        ClozeNoteRepository clozeNoteRepository = new(dbContext);

        await clozeNoteRepository.InsertArticleElementAsync(clozeNote);

        ClozeNote updatedClozeNote = dbContext.ClozeNotes.First(cn => cn.Id == clozeNote.Id);
        Assert.Equal("World2Cloze", updatedClozeNote.Text);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 11));
    }
}