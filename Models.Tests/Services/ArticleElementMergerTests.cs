using AnkiBooks.Models;
using AnkiBooks.Models.Services;
using AnkiBooks.Models.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.Models.Tests;

public class ArticleElementMergerTests
{
    [Fact]
    public void TestWithEmptyBasicNotes()
    {
        List<BasicNote> basicNotes = [];
        List<ClozeNote> clozeNotes =
        [
            new() { Text="a", OrdinalPosition=0 },
            new() { Text="b", OrdinalPosition=1 }
        ];

        IList<IArticleElement> result = ArticleElementMerger.ElementsOrdered(basicNotes, clozeNotes);

        Assert.True(CountIsCorrectAndElementsAreOrdered(result, 2));
    }

    [Fact]
    public void TestWithBothKinds()
    {
        List<BasicNote> basicNotes =
        [
            new() { Front="a", Back="b", OrdinalPosition=1 },
            new() { Front="a", Back="b", OrdinalPosition=3 },
            new() { Front="a", Back="b", OrdinalPosition=4 }
        ];
        List<ClozeNote> clozeNotes =
        [
            new() { Text="a", OrdinalPosition=0 },
            new() { Text="b", OrdinalPosition=2 }
        ];

        IList<IArticleElement> result = ArticleElementMerger.ElementsOrdered(basicNotes, clozeNotes);

        Assert.True(CountIsCorrectAndElementsAreOrdered(result, 5));
    }

    private static bool CountIsCorrectAndElementsAreOrdered(IList<IArticleElement> elements, int expectedCount)
    {
        if (elements.Count != expectedCount)
        {
            return false;
        }

        for(int i = 0; i < elements.Count; i ++)
        {
            if (i != elements[i].OrdinalPosition)
            {
                return false;
            }
        }

        return true;
    }
}