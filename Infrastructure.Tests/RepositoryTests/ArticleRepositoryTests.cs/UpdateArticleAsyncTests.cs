using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ArticleRepositoryTests;

public class UpdateArticleAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task ParentArticleIdGetsUpdated()
    {
        using var dbContext = InMemoryDbContext();

        Article parent = new("Parent article");
        Article child = new("Child article");

        dbContext.Articles.Add(parent);
        dbContext.Articles.Add(child);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        ArticleRepository articleRepository = new(dbContext);
        child.ParentArticleId = parent.Id;

        await articleRepository.UpdateArticleAsync(child);

        dbContext.ChangeTracker.Clear();

        Article updatedArticle = dbContext.Articles.First(a => a.Id == child.Id);
        Assert.Equal(parent.Id, updatedArticle.ParentArticleId);
    }
}