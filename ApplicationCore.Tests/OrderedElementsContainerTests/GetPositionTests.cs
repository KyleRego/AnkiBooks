using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class GetPositionTests
{
    [Fact]
    public void TestGettingOrdinalPositionsOfElements()
    {
        Deck d1 = new() { OrdinalPosition=0 };
        Deck d2 = new() { OrdinalPosition=1 };
        Deck d3 = new() { OrdinalPosition=2 };
        Deck d4 = new() { OrdinalPosition=3 };
        Deck d5 = new() { OrdinalPosition=4 };
        List<ArticleElement> notes = [ d1, d2, d3, d4, d5 ];

        OrderedElementsContainer<ArticleElement> container = new(notes);

        Assert.Equal(0, container.GetPosition(d1));
        Assert.Equal(1, container.GetPosition(d2));
        Assert.Equal(2, container.GetPosition(d3));
        Assert.Equal(3, container.GetPosition(d4));
        Assert.Equal(4, container.GetPosition(d5));
    }
}