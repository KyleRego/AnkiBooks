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
        ClozeNote n1 = new() { Text="a", OrdinalPosition=0 };
        BasicNote n2 = new() { Front="a", Back="b", OrdinalPosition=1 };
        ClozeNote n3 = new() { Text="b", OrdinalPosition=2 };
        BasicNote n4 = new() { Front="a", Back="b", OrdinalPosition=3 };
        BasicNote n5 = new() { Front="a", Back="b", OrdinalPosition=4 };
        Section section = new("Test section")
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

        OrderedElementsContainer container = new(section.OrderedElements());

        Assert.Equal(0, container.GetPosition(n1));
        Assert.Equal(1, container.GetPosition(n2));
        Assert.Equal(2, container.GetPosition(n3));
        Assert.Equal(3, container.GetPosition(n4));
        Assert.Equal(4, container.GetPosition(n5));
    }
}