using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.WebApp.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class DeleteBasicNoteAsyncTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
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

        await basicNoteRepository.DeleteBasicNoteAsync(noteToDelete);

        Assert.Null(dbContext.BasicNotes.FirstOrDefault(bn => bn.Id == noteToDelete.Id));
        article = dbContext.Articles.Include(a => a.BasicNotes)
                                    .Include(a => a.ClozeNotes)
                                    .First(a => a.Id == article.Id);
        Assert.Equal(9, article.ElementsCount());
        Assert.True(article.ElementOrdinalPositionsAreCorrect());
    }
}