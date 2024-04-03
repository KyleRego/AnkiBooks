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
                                    List<ClozeNote> orderedClozeNotes)
    {
        OrderedElements = InitializeOrderedElementsFromOrdered(orderedBasicNotes, orderedClozeNotes);
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

    private static List<IArticleElement> InitializeOrderedElementsFromOrdered(List<BasicNote> orderedBasicNotes,
                                                                            List<ClozeNote> orderedClozeNotes)
    {
        if (orderedBasicNotes.Count == 0)
        {
            return orderedClozeNotes.Cast<IArticleElement>().ToList();
        }

        if (orderedClozeNotes.Count == 0)
        {
            return orderedBasicNotes.Cast<IArticleElement>().ToList();
        }
        
        List<IArticleElement> result = [];
        
        int finalBnIndex = orderedBasicNotes.Count - 1;
        int finalCnIndex = orderedClozeNotes.Count - 1;
        int currentBnIndex = 0;
        int currentCnIndex = 0;

        while (currentBnIndex <= finalBnIndex && currentCnIndex <= finalCnIndex)
        {
            BasicNote currentBn = orderedBasicNotes[currentBnIndex];
            ClozeNote currentCn = orderedClozeNotes[currentCnIndex];

            if (currentBn.OrdinalPosition > currentCn.OrdinalPosition)
            {
                result.Add(currentCn);
                currentCnIndex += 1;
            }
            else
            {
                result.Add(currentBn);
                currentBnIndex += 1;
            }
        }

        while (currentBnIndex <= finalBnIndex)
        {
            result.Add(orderedBasicNotes[currentBnIndex]);
            currentBnIndex += 1;
        }

        while (currentCnIndex <= finalCnIndex)
        {
            result.Add(orderedClozeNotes[currentCnIndex]);
            currentCnIndex += 1;
        }

        return result;
    }
}