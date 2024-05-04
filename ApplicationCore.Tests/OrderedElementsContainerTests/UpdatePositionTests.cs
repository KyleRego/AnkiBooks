using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class AdjustElementPositionTests
{
    [Fact]
    public void AdjustElementPositionMovesElementToItsPositionCorrectly1()
    {
        Deck n1 = new() { OrdinalPosition=0 };
        Deck n2 = new() { OrdinalPosition=1 };
        Deck n3 = new() { OrdinalPosition=2 };
        Deck n4 = new() { OrdinalPosition=3 };
        Deck n5 = new() { OrdinalPosition=4 };
        List<ArticleElement> notes = [n1, n2, n3, n4, n5];

        OrderedElementsContainer<ArticleElement> container = new(notes);

        n2.OrdinalPosition = 0;
        container.UpdatePosition(n2);

        IOrdinalChild elementAtZero = container.ElementAtPosition(0);
        Assert.Equal(n2, elementAtZero);
        container.AssertNotesAreOrdered();

        n5.OrdinalPosition = 0;
        container.UpdatePosition(n5);

        IOrdinalChild elementAtFour = container.ElementAtPosition(4);
        Assert.Equal(n4, elementAtFour);
        container.AssertNotesAreOrdered();
    }

    [Fact]
    public void AdjustElementPositionMovesElementToItsPositionCorrectly2()
    {
        Deck n1 = new() { OrdinalPosition=0 };
        Deck n2 = new() { OrdinalPosition=1 };
        Deck n3 = new() { OrdinalPosition=2 };
        Deck n4 = new() { OrdinalPosition=3 };
        Deck n5 = new() { OrdinalPosition=4 };
        List<ArticleElement> notes = [n1, n2, n3, n4, n5];

        OrderedElementsContainer<ArticleElement> container = new(notes);

        n1.OrdinalPosition = 3;
        container.UpdatePosition(n1);

        IOrdinalChild elementAtThree = container.ElementAtPosition(3);
        Assert.Equal(n1, elementAtThree);
        container.AssertNotesAreOrdered();

        n2.OrdinalPosition = 4;
        container.UpdatePosition(n2);

        IOrdinalChild elementAtFour = container.ElementAtPosition(4);
        Assert.Equal(n2, elementAtFour);
        container.AssertNotesAreOrdered();
    }
}