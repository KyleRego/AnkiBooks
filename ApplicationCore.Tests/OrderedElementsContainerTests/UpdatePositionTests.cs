using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class AdjustElementPositionTests
{
    [Fact]
    public void AdjustElementPositionMovesElementToItsPositionCorrectly1()
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

        OrderedElementsContainer container = new(article.OrderedElements());

        n2.OrdinalPosition = 0;
        container.UpdatePosition(n2);

        IArticleElement elementAtZero = container.ElementAtPosition(0);
        Assert.Equal(n2, elementAtZero);
        container.ExpectElementsAreOrdered();

        n5.OrdinalPosition = 0;
        container.UpdatePosition(n5);

        IArticleElement elementAtFour = container.ElementAtPosition(4);
        Assert.Equal(n4, elementAtFour);
        container.ExpectElementsAreOrdered();
    }

    [Fact]
    public void AdjustElementPositionMovesElementToItsPositionCorrectly2()
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

        OrderedElementsContainer container = new(article.OrderedElements());

        n1.OrdinalPosition = 3;
        container.UpdatePosition(n1);

        IArticleElement elementAtThree = container.ElementAtPosition(3);
        Assert.Equal(n1, elementAtThree);
        container.ExpectElementsAreOrdered();

        n2.OrdinalPosition = 4;
        container.UpdatePosition(n2);

        IArticleElement elementAtFour = container.ElementAtPosition(4);
        Assert.Equal(n2, elementAtFour);
        container.ExpectElementsAreOrdered();
    }
}