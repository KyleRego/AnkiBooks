using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Identity;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.UserArticleRepositoryTests;

public class GetArticleAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task NotesAndContentsAreEagerLoadedWithArticle()
    {
        using var dbContext = InMemoryDbContext();

        ApplicationUser user = new();
        Article art = new("Test article with basic notes, cloze notes, and markdown contents")
        {
            User = user,
            BasicNotes = [ new() { Front="a", Back="b", OrdinalPosition=0 } ],
            ClozeNotes = [ new() { Text="aaa", OrdinalPosition=1 } ],
            MarkdownContents = [ new() { Text="adfj", OrdinalPosition=2 } ]
        };
        dbContext.Articles.Add(art);
        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        UserArticleRepository articleRepository = new(dbContext);

        Article? result = await articleRepository.GetArticleAsync(user.Id, art.Id);

        Assert.Single(result!.BasicNotes);
        Assert.Single(result.ClozeNotes);
        Assert.Single(result.MarkdownContents);
    }

    [Fact]
    public async Task ArticleIsNotReturnedWhenUserIdIsDifferent()
    {
        using var dbContext = InMemoryDbContext();

        ApplicationUser user = new();
        Article art = new("Test article") { User = user };

        dbContext.Articles.Add(art);
        await dbContext.SaveChangesAsync();
        dbContext.ChangeTracker.Clear();

        UserArticleRepository articleRepository = new(dbContext);

        Article? result = await articleRepository.GetArticleAsync("qwertyui", art.Id);
        Assert.Null(result);
    }
}