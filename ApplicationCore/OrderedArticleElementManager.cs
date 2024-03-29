using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore;

/// <summary>
/// Wraps an article and a list of the article's polymorphic (IArticleElement) elements
/// The article passed to the constructor must have its article element properties sorted
/// already for the combining of those into a single ordered list to work with the algorithm
/// </summary>
public class OrderedArticleElementsManager
{
    private readonly Article _article;
    public List<IArticleElement> OrderedElements { get; }

    public OrderedArticleElementsManager(Article article)
    {
        _article = article;
        OrderedElements = InitializeOrderedElements();
    }

    private List<IArticleElement> InitializeOrderedElements()
    {
        List<BasicNote> orderedBasicNotes = _article.BasicNotes;
        List<ClozeNote> orderedClozeNotes = _article.ClozeNotes;

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