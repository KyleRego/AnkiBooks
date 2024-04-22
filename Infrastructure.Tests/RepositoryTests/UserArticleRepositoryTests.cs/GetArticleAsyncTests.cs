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
            User = user
        };
        Section sec = new() { OrdinalPosition=0 } ;
        BasicNote bn = new() { Front="a", Back="b", OrdinalPosition=0 };
        ClozeNote cn = new() { Text="aaa", OrdinalPosition=1 };
        MarkdownContent mdC = new() { Text="adfj", OrdinalPosition=0 };
        art.Sections.Add(sec);
        sec.BasicNotes.Add(bn);
        sec.ClozeNotes.Add(cn);
        sec.MarkdownContents.Add(mdC);
        dbContext.Articles.Add(art);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        UserArticleRepository articleRepository = new(dbContext);

        Article? result = await articleRepository.GetArticleAsync(user.Id, art.Id);
        Assert.NotNull(result);
        Section? section = result.Sections.First();
        Assert.NotNull(section);
        Assert.Single(section.BasicNotes);
        Assert.Single(section.ClozeNotes);
        Assert.Single(section.MarkdownContents);
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