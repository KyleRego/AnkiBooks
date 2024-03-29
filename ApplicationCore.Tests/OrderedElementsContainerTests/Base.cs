using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

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