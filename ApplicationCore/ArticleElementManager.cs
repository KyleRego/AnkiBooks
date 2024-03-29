using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore;

/// <summary>
/// Wraps an article and a list of the article's polymorphic (IArticleElement) elements
/// The article passed to the constructor must have its article element properties sorted
/// already for the combining of those into a single ordered list to work with the algorithm
/// </summary>
public class ArticleElementManager
{
    private readonly Article _article;
    public List<IArticleElement> OrderedElements { get; }

    public ArticleElementManager(Article article)
    {
        _article = article;
        OrderedElements = SetupOrderedElements();
    }

    private List<IArticleElement> SetupOrderedElements()
    {
        return OrderedArticleElementsMerger.ElementsOrdered(_article.BasicNotes, _article.ClozeNotes);
    }
}

public class OrderedArticleElementsMerger
{
    public static List<IArticleElement> ElementsOrdered(List<BasicNote> orderedBasicNotes, List<ClozeNote> orderedClozeNotes)
    {
        // Consider checking that the input is ordered, throw an error if not
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