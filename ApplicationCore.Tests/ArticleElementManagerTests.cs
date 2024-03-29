using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.ApplicationCore.Tests;

public class ArticleElementManagerTests
{
    [Fact]
    public void TestConstructorWithOnlyClozeNotes()
    {
        Article article = new("Test article")
        {
            BasicNotes = [],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=1 }
            ]
        };

        OrderedArticleElementsManager manager = new(article);
        
        Assert.True(NumberOfElementsIsCorrect(manager, 2));
        Assert.True(ElementsAreCorrectlyOrdered(manager));
    }

    [Fact]
    public void TestConstructorWithBasicAndClozeNotes()
    {
        Article article = new("Test article")
        {
            BasicNotes =
            [
                new() { Front="a", Back="b", OrdinalPosition=1 },
                new() { Front="a", Back="b", OrdinalPosition=3 },
                new() { Front="a", Back="b", OrdinalPosition=4 }
            ],
                ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=2 }
            ]
        };

        OrderedArticleElementsManager manager = new(article);

        Assert.True(NumberOfElementsIsCorrect(manager, 5));
        Assert.True(ElementsAreCorrectlyOrdered(manager));
    }

    private static bool NumberOfElementsIsCorrect(OrderedArticleElementsManager manager, int expectedCount)
    {
        if (manager.OrderedElements.Count != expectedCount)
        {
            return false;
        }

        return true;
    }

    private static bool ElementsAreCorrectlyOrdered(OrderedArticleElementsManager manager)
    {
        List<IArticleElement> elements = manager.OrderedElements;

        for (int i = 0; i < elements.Count; i++)
        {
            if (i != elements[i].OrdinalPosition)
            {
                return false;
            }
        }

        return true;
    }
}