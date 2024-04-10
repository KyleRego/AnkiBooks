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
        Article article = new("Test article")
        {
            BasicNotes = [
                new() { Front="a", Back="b", OrdinalPosition=2}
            ],
            ClozeNotes =
            [
                new() { Text="a", OrdinalPosition=0 },
                new() { Text="b", OrdinalPosition=1 }
            ],
            MarkdownContents =
            [
                new() { Text="a", OrdinalPosition=4},
                new() { Text="a", OrdinalPosition=5}
            ]
        };

        OrderedElementsContainer container = new(article.OrderedElements());
        IArticleElement firstElementToRemove = container.ElementAtPosition(2);
        container.Remove(firstElementToRemove);
        container.ExpectElementsCountIs(4);
        container.ExpectElementsAreOrdered();

        IArticleElement secondElementToRemove = container.ElementAtPosition(0);
        container.Remove(secondElementToRemove);
        container.ExpectElementsCountIs(3);
        container.ExpectElementsAreOrdered();
    }
}