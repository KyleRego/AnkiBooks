using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Tests.OrderedElementsContainerTests;

public class Base
{
    protected static bool NumberOfElementsIsCorrect(OrderedElementsContainer manager, int expectedCount)
    {
        if (manager.OrderedElements.Count != expectedCount)
        {
            return false;
        }

        return true;
    }

    protected static bool ElementsAreCorrectlyOrdered(OrderedElementsContainer manager)
    {
        List<IArticleElement> elements = manager.OrderedElements;

        for (int i = 0; i < elements.Count; i++)
        {
            if (i != elements[i].OrdinalPosition)
            {
                return false;
            }
        }

        return true;
    }
}