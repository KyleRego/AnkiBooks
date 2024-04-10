using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests;

public class ArticleTests
{
    [Fact]
    public void OrderedElementsReturnsElementsOfArticleOrdered1()
    {
        Article article = new("Test article")
        {
            BasicNotes = [],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=1 }
            ],
            MarkdownContents = []
        };

        Assert.Equal(article.ClozeNotes, article.OrderedElements());
    }

    [Fact]
    public void OrderedElementsReturnsElementsOfArticleOrdered2()
    {
        Article article = new("Test article")
        {
            BasicNotes = [
                new() { Front="a", Back="b", OrdinalPosition=2 },
                new() { Front="a", Back="b", OrdinalPosition=4 }
            ],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=1 }
            ],
            MarkdownContents = [
                new() { Text="a", OrdinalPosition=3 }
            ]
        };

        List<IArticleElement> result = article.OrderedElements();
        for (int i = 0; i < result.Count; i++ )
        {
            Assert.Equal(i, result[i].OrdinalPosition);
        }
    }

    [Fact]
    public void OrderedElementsReturnsElementsOfArticleOrderedWhenNotAlreadyOrdered()
    {
        Article article = new("Test article")
        {
            BasicNotes = [
                new() { Front="a", Back="b", OrdinalPosition=4 },
                new() { Front="a", Back="b", OrdinalPosition=2 }
            ],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=1 },
                new() { Text="b", OrdinalPosition=0 }
            ],
            MarkdownContents = [
                new() { Text="a", OrdinalPosition=3 }
            ]
        };

        List<IArticleElement> result = article.OrderedElements();
        for (int i = 0; i < result.Count; i++ )
        {
            Assert.Equal(i, result[i].OrdinalPosition);
        }   
    }
}