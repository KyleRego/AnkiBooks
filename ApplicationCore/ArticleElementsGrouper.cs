using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore;

public static class ArticleElementsGrouper
{
    // Returns list of tuples representing the groups of contents followed by notes
    public static List<(List<IContent>, List<INote>)> Groups(List<ArticleElement> orderedElements)
    {
        List<(List<IContent>, List<INote>)> result = [];

        List<IContent> contents = [];
        List<INote> notes = [];

        bool populatingContents = true;

        foreach (ArticleElement element in orderedElements)
        {
            if (populatingContents == true)
            {
                if (element is IContent content)
                {
                    contents.Add(content);
                }
                else if (element is INote note)
                {
                    populatingContents = false;
                    notes.Add(note);
                }
            }
            else
            {
                if (element is IContent content)
                {
                    result.Add((contents, notes));
                    contents = [];
                    notes = [];
                    populatingContents = true;
                    contents.Add(content);
                }
                else if (element is INote note)
                {
                    notes.Add(note);
                }
            }
        }

        if (contents.Count != 0 || notes.Count != 0)
        {
            result.Add((contents, notes));
        }

        return result;
    }
}