using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ArticleRepositoryTests;

public class GetArticleAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task NotesAndContentsAreEagerLoadedWithArticle()
    {
        using var dbContext = InMemoryDbContext();

        Article art = new("Test article with basic notes, cloze notes, and markdown contents");
        Section sec = new("Section with basic notes, cloze notes, and markdown contents");
        BasicNote bn = new() { Front="a", Back="b", OrdinalPosition=0 };
        ClozeNote cn = new() { Text="aaa", OrdinalPosition=1 };
        MarkdownContent mdC = new() { Text="adfj", OrdinalPosition=0 };
        art.Sections.Add(sec);
        sec.BasicNotes.Add(bn);
        sec.ClozeNotes.Add(cn);
        sec.MarkdownContents.Add(mdC);
        dbContext.Add(art);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        ArticleRepository articleRepository = new(dbContext);

        Article? result = await articleRepository.GetArticleAsync(art.Id);
        Assert.NotNull(result);
        Section? section = result.Sections.First();
        Assert.NotNull(section);
        Assert.Single(section.BasicNotes);
        Assert.Single(section.ClozeNotes);
        Assert.Single(section.MarkdownContents);
    }
}