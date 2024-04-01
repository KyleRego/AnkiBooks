using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.WebApp.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class UpdateArticleElementAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task LastElementIsMovedToFirstPosition()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
        ClozeNote noteToUpdate = article.ClozeNotes.First(bn => bn.OrdinalPosition == 9);

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        ClozeNote clozeNote = new()
        {
            Id = noteToUpdate.Id,
            Text = "4321",
            OrdinalPosition = 0,
            ArticleId = article.Id
        };
        ClozeNoteRepository clozeNoteRepository = new(dbContext);

        await clozeNoteRepository.UpdateArticleElementAsync(clozeNote);

        ClozeNote updatedClozeNote = dbContext.ClozeNotes.First(cn => cn.Id == clozeNote.Id);
        Assert.Equal("4321", updatedClozeNote.Text);
        Assert.Equal(0, updatedClozeNote.OrdinalPosition);
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 10));
    }
}