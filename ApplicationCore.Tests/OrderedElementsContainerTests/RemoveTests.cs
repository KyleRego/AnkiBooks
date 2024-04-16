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
        Section section = new("Test article")
        {
            BasicNotes = [
                new() { Front="a", Back="b", OrdinalPosition=2},
                new() { Front="a", Back="b", OrdinalPosition=3},
                new() { Front="a", Back="b", OrdinalPosition=4}
            ],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=1 }
            ]
        };

        OrderedElementsContainer container = new(section.OrderedNotes().Cast<IOrdinalChild>().ToList());
        IOrdinalChild firstElementToRemove = container.ElementAtPosition(2);
        container.Remove(firstElementToRemove);
        container.ExpectElementsCountIs(4);
        container.ExpectElementsAreOrdered();

        IOrdinalChild secondElementToRemove = container.ElementAtPosition(0);
        container.Remove(secondElementToRemove);
        container.ExpectElementsCountIs(3);
        container.ExpectElementsAreOrdered();
    }
}