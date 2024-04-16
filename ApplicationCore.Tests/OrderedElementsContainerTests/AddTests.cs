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
        Section section = new("Test article")
        {
            BasicNotes = [],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=1 }
            ]
        };

        OrderedElementsContainer<INote> container = new(section.OrderedNotes());
        BasicNote noteToAdd = new() { Front="a", Back="b", OrdinalPosition=1 };
        container.Add(noteToAdd);
        container.AssertNotesCountIs(3);
        container.AssertNotesAreOrdered();

        IOrdinalChild element = container.OrderedElements.First(el => el.OrdinalPosition == 1);
        Assert.Equal(noteToAdd, element);
    }
}