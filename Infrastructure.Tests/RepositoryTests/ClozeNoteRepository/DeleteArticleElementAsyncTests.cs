using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class DeleteArticleElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task BasicNoteInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        ClozeNote noteToDelete = article.ClozeNotes.First(cn => cn.OrdinalPosition == 3);

        ClozeNoteRepository clozeNoteRepository = new(dbContext);

        await clozeNoteRepository.DeleteArticleElementAsync(noteToDelete);

        Assert.Null(dbContext.ClozeNotes.FirstOrDefault(cn => cn.Id == noteToDelete.Id));
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 9));
    }
}