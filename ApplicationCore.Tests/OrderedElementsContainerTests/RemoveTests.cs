using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class RemoveTests
{
    [Fact]
    public void RemoveRemovesElementAndLeavesOrdinalPositionsValid()
    {
        List<ArticleElement> notes =  [
            new ClozeNote() { Text="a", OrdinalPosition=0 },
            new ClozeNote() { Text="b", OrdinalPosition=1 },
            new BasicNote() { Front="a", Back="b", OrdinalPosition=2},
            new BasicNote() { Front="a", Back="b", OrdinalPosition=3},
            new BasicNote() { Front="a", Back="b", OrdinalPosition=4}
        ];

        OrderedElementsContainer<ArticleElement> container = new(notes);
        ArticleElement firstElementToRemove = container.ElementAtPosition(2);
        container.Remove(firstElementToRemove);
        container.AssertNotesCountIs(4);
        container.AssertNotesAreOrdered();

        ArticleElement secondElementToRemove = container.ElementAtPosition(0);
        container.Remove(secondElementToRemove);
        container.AssertNotesCountIs(3);
        container.AssertNotesAreOrdered();
    }
}