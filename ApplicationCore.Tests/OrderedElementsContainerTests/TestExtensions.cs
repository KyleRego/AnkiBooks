using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public static class OrderedElementsContainerTestExtensions
{
    public static void ExpectElementsCountIs(this OrderedElementsContainer elementsContainer, int expectedCount)
    {
        Assert.True(elementsContainer.OrderedElements.Count == expectedCount);
    }

    public static void ExpectElementsAreOrdered(this OrderedElementsContainer elementsContainer)
    {
        List<IOrderedElement> elements = elementsContainer.OrderedElements;

        for (int i = 0; i < elements.Count; i++)
        {
            Assert.True(i == elements[i].OrdinalPosition);
        }
    }
}