using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class GetOrdinalPositionTests : Base
{
    [Fact]
    public void TestGettingOrdinalPositionsOfElements()
    {
        ClozeNote n1 = new() { Text="a", OrdinalPosition=0 };
        BasicNote n2 = new() { Front="a", Back="b", OrdinalPosition=1 };
        ClozeNote n3 = new() { Text="b", OrdinalPosition=2 };
        BasicNote n4 = new() { Front="a", Back="b", OrdinalPosition=3 };
        BasicNote n5 = new() { Front="a", Back="b", OrdinalPosition=4 };
        Article article = new("Test article")
        {
            BasicNotes =
            [
                n2,
                n4,
                n5
            ],
                ClozeNotes =
            [
                n1,
                n3
            ]
        };

        OrderedElementsContainer container = new(article.BasicNotes, article.ClozeNotes, []);

        Assert.Equal(0, container.GetOrdinalPosition(n1));
        Assert.Equal(1, container.GetOrdinalPosition(n2));
        Assert.Equal(2, container.GetOrdinalPosition(n3));
        Assert.Equal(3, container.GetOrdinalPosition(n4));
        Assert.Equal(4, container.GetOrdinalPosition(n5));
    }
}