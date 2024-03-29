using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class AddTests : Base
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

        OrderedElementsContainer container = new(article.BasicNotes, article.ClozeNotes);
        BasicNote noteToAdd = new() { Front="a", Back="b", OrdinalPosition=1 };
        container.Add(noteToAdd);
        Assert.True(ElementsAreCorrectlyOrdered(container));
        Assert.Equal(1, container.GetOrdinalPosition(noteToAdd));
    }
}