using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public static class OrderedElementsContainerTestExtensions
{
    public static void AssertNotesCountIs(this OrderedElementsContainer<ArticleElement> elementsContainer, int expectedCount)
    {
        Assert.True(elementsContainer.OrderedElements.Count == expectedCount);
    }

    public static void AssertNotesAreOrdered(this OrderedElementsContainer<ArticleElement> elementsContainer)
    {
        List<ArticleElement> elements = elementsContainer.OrderedElements;

        for (int i = 0; i < elements.Count; i++)
        {
            Assert.True(i == elements[i].OrdinalPosition);
        }
    }
}