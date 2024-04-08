using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore;

/// <summary>
/// Wraps an article and a list of the article's polymorphic (IArticleElement) elements
/// The article passed to the constructor must have its article element properties sorted
/// already for the combining of those into a single ordered list to work with the algorithm
/// </summary>
public class OrderedElementsContainer
{
    public List<IArticleElement> OrderedElements { get; }

    public OrderedElementsContainer(List<BasicNote> orderedBasicNotes,
                                    List<ClozeNote> orderedClozeNotes,
                                    List<MarkdownContent> orderedMarkdownContents)
    {
        OrderedElements = InitializeOrderedElementsFromOrdered( orderedBasicNotes,
                                                                orderedClozeNotes,
                                                                orderedMarkdownContents);
    }

    public int GetOrdinalPosition(IArticleElement element)
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

    public void AddElementAndRemoveFromOldPosition(IArticleElement element, int oldPosition)
    {
        OrderedElements.RemoveAt(oldPosition);
        foreach (IArticleElement el in OrderedElements.Where(el => el.OrdinalPosition >= oldPosition))
        {
            el.OrdinalPosition -= 1;
        }
        Add(element);
    }

    private static List<IArticleElement> InitializeOrderedElementsFromOrdered(List<BasicNote> orderedBasicNotes,
                                                                            List<ClozeNote> orderedClozeNotes,
                                                                            List<MarkdownContent> orderedMarkdownContents)
    {
        List<IArticleElement> result = orderedBasicNotes.Cast<IArticleElement>()
            .Concat(orderedClozeNotes.Cast<IArticleElement>())
            .Concat(orderedMarkdownContents.Cast<IArticleElement>())
            .OrderBy(item => item.OrdinalPosition)
            .ToList();

        return result;
    }
}