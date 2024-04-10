using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore;

public class OrderedElementsContainer(List<IArticleElement> orderedElements)
{
    public List<IArticleElement> OrderedElements { get; } = orderedElements;

    public int GetPosition(IArticleElement element)
    {
        for (int i = 0; i < OrderedElements.Count; i++)
        {
            if (element.Id == OrderedElements[i].Id)
            {
                return i;
            } 
        }
        // TODO: consider handling this better
        throw new ApplicationException();
    }

    public IArticleElement ElementAtPosition(int position)
    {
        return OrderedElements[position];
    }

    public int Count()
    {
        return OrderedElements.Count;
    }

    public void Add(IArticleElement element)
    {
        foreach (IArticleElement el in OrderedElements.Where(el => el.OrdinalPosition >= element.OrdinalPosition))
        {
            el.OrdinalPosition += 1;
        }
        OrderedElements.Insert(element.OrdinalPosition, element);
    }

    public void Remove(IArticleElement element)
    {
        OrderedElements.Remove(element);
        foreach (IArticleElement el in OrderedElements.Where(el => el.OrdinalPosition >= element.OrdinalPosition))
        {
            el.OrdinalPosition -= 1;
        }
    }

    public void UpdatePosition(IArticleElement element)
    {
        // TODO: This can be done in a better way
        int newPosition = element.OrdinalPosition;
        int oldPosition = GetPosition(element);

        element.OrdinalPosition = oldPosition;
        Remove(element);
        element.OrdinalPosition = newPosition;
        Add(element);
    }
}