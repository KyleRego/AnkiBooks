using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.WebApp.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class DeleteArticleElementAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task BasicNoteInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        Article article = await ArticleFactory.ArticleWithTenAlternatingBasicAndClozeNotes(_factory);
        BasicNote noteToDelete = article.BasicNotes.First(bn => bn.OrdinalPosition == 4);

        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.DeleteArticleElementAsync(noteToDelete);

        Assert.Null(dbContext.BasicNotes.FirstOrDefault(bn => bn.Id == noteToDelete.Id));
        Assert.True(ArticleValidator.CorrectElementsCountAndOrdinalPositions(dbContext, article, 9));
    }
}