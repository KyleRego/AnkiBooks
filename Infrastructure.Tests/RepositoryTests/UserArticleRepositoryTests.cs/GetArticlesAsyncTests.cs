using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Identity;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.UserArticleRepositoryTests;

public class GetArticlesAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task RootArticlesAreReturnedWithNestedArticles()
    {
        using var dbContext = InMemoryDbContext();

        ApplicationUser user = new();
        Article rootArticle = new("Root article")
        {
            ChildArticles = [
                new("Child of root article")
                {
                    ChildArticles = [
                        new("Child of child of root article")
                    ]
                }
            ],
            User = user
        };
        dbContext.Articles.Add(rootArticle);
        await dbContext.SaveChangesAsync();

        UserArticleRepository articleRepository = new(dbContext);

        List<Article> result = await articleRepository.GetArticlesAsync(user.Id);
        Assert.Single(result);
        Assert.Single(result.First().ChildArticles);
        Assert.Single(result.First().ChildArticles.First().ChildArticles);
    }
}