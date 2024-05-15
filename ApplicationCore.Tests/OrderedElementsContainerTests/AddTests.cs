using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class AddTests
{
    [Fact]
    public void TestAddWithTwoElementsAlready()
    {
        List<ArticleElement> decks =
        [
            new Deck() {  OrdinalPosition=0 },
            new Deck() {  OrdinalPosition=1 }
        ];

        OrderedElementsContainer<ArticleElement> container = new(decks);
        Deck deck = new() { OrdinalPosition=1 };
        container.Add(deck);
        container.AssertNotesCountIs(3);
        container.AssertNotesAreOrdered();

        IOrdinalChild element = container.OrderedElements.First(el => el.OrdinalPosition == 1);
        Assert.Equal(deck, element);
    }
}