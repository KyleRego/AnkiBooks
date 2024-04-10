using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class DeleteArticleElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task BasicNoteInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticleWithTenAlternatingBasicAndClozeNotes();
        BasicNote noteToDelete = article.BasicNotes.First(bn => bn.OrdinalPosition == 4);

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.DeleteArticleElementAsync(noteToDelete);

        Assert.Null(dbContext.BasicNotes.FirstOrDefault(bn => bn.Id == noteToDelete.Id));
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 9));
    }
}