using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Services;

// This is not really a service in .NET if not using DI?
// TODO: Maybe change the name or add it to the service collection
public class ArticleElementMerger
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