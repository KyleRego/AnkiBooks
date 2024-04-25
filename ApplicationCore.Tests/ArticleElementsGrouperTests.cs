using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests;

public class ArticleElementsGrouperTests
{
    [Fact]
    public void WhenFirstElementsAreContents()
    {
        List<ArticleElement> orderedElements =
        [
            new MarkdownContent() { Text="t", OrdinalPosition=0 },
            new MarkdownContent() { Text="d", OrdinalPosition=1 },
            new MarkdownContent() { Text="f", OrdinalPosition=2 },
            new BasicNote() { Front="a", Back="b", OrdinalPosition=3 },
            new MarkdownContent() { Text="f", OrdinalPosition=4 },
            new ClozeNote() { Text="g", OrdinalPosition=5 }
        ];

        List<(List<IContent>, List<INote>)> result = ArticleElementsGrouper.Groups(orderedElements);
        Assert.Equal(2, result.Count);
        (List<IContent> firstContents, List<INote> firstNotes) = result.First();
        Assert.Equal(3, firstContents.Count);
        Assert.Single(firstNotes);
        (List<IContent> secondContents, List<INote> secondNotes) = result[1];
        Assert.Single(secondContents);
        Assert.Single(secondNotes);
    }

    [Fact]
    public void WhenFirstElementsAreNotesFirstGroupHasNoContents()
    {
        List<ArticleElement> orderedElements =
        [
            new BasicNote() { Front="a", Back="b", OrdinalPosition=0 },
            new ClozeNote() { Text="a", OrdinalPosition=1 },
            new MarkdownContent() { Text="t", OrdinalPosition=2 }
        ];

        List<(List<IContent>, List<INote>)> result = ArticleElementsGrouper.Groups(orderedElements);
        Assert.Equal(2, result.Count);
        (List<IContent> firstContents, List<INote> firstNotes) = result.First();
        Assert.Empty(firstContents);
        Assert.Equal(2, firstNotes.Count);
        (List<IContent> secondContents, List<INote> secondNotes) = result[1];
        Assert.Single(secondContents);
        Assert.Empty(secondNotes);

    }
}