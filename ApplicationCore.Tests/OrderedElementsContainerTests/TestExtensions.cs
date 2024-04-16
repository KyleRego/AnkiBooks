using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public static class OrderedElementsContainerTestExtensions
{
    public static void AssertNotesCountIs(this OrderedElementsContainer<INote> elementsContainer, int expectedCount)
    {
        Assert.True(elementsContainer.OrderedElements.Count == expectedCount);
    }

    public static void AssertNotesAreOrdered(this OrderedElementsContainer<INote> elementsContainer)
    {
        List<INote> elements = elementsContainer.OrderedElements;

        for (int i = 0; i < elements.Count; i++)
        {
            Assert.True(i == elements[i].OrdinalPosition);
        }
    }
}