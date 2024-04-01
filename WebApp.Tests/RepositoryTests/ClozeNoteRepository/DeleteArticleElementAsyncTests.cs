using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.WebApp.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class DeleteArticleElementAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task BasicNoteInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
        ClozeNote noteToDelete = article.ClozeNotes.First(cn => cn.OrdinalPosition == 3);

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        ClozeNoteRepository clozeNoteRepository = new(dbContext);

        await clozeNoteRepository.DeleteArticleElementAsync(noteToDelete);

        Assert.Null(dbContext.ClozeNotes.FirstOrDefault(cn => cn.Id == noteToDelete.Id));
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 9));
    }
}