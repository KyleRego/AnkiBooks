using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ArticleRepositoryTests;

public class GetArticlesAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task RootArticlesAreReturnedWithNestedArticles()
    {
        using var dbContext = InMemoryDbContext();

        Article rootArticle = new("Root article")
        {
            ChildArticles = [
                new("Child of root article")
                {
                    ChildArticles = [
                        new("Child of child of root article")
                    ]
                }
            ]
        };
        dbContext.Articles.Add(rootArticle);
        await dbContext.SaveChangesAsync();

        ArticleRepository articleRepository = new(dbContext);

        List<Article> result = await articleRepository.GetArticlesAsync();
        Assert.Single(result);
        Assert.Single(result.First().ChildArticles);
        Assert.Single(result.First().ChildArticles.First().ChildArticles);
    }
}