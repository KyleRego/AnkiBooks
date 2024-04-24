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
        List<ArticleElement> notes =
        [
            new ClozeNote() { Text="a", OrdinalPosition=0 },
            new ClozeNote() { Text="b", OrdinalPosition=1 }
        ];

        OrderedElementsContainer<ArticleElement> container = new(notes);
        BasicNote noteToAdd = new() { Front="a", Back="b", OrdinalPosition=1 };
        container.Add(noteToAdd);
        container.AssertNotesCountIs(3);
        container.AssertNotesAreOrdered();

        IOrdinalChild element = container.OrderedElements.First(el => el.OrdinalPosition == 1);
        Assert.Equal(noteToAdd, element);
    }
}