using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class UpdateArticleElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task LastElementIsMovedToFirstPosition()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        ClozeNote noteToUpdate = article.ClozeNotes.First(bn => bn.OrdinalPosition == 9);

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