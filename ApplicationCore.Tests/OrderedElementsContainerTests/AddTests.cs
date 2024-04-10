using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class AddTests
{
    [Fact]
    public void TestAddWithTwoElementsAlready()
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

        OrderedElementsContainer container = new(article.OrderedElements());
        BasicNote noteToAdd = new() { Front="a", Back="b", OrdinalPosition=1 };
        container.Add(noteToAdd);
        container.ExpectElementsCountIs(3);
        container.ExpectElementsAreOrdered();

        IArticleElement element = container.OrderedElements.First(el => el.OrdinalPosition == 1);
        Assert.Equal(noteToAdd, element);
    }
}