using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.Infrastructure.Tests.Helpers;

public static class SectionValidator
{
    public static bool CorrectElementsCountAndOrdinalPositions(
        ApplicationDbContext dbContext, Section section, int expectedCount)
    {
        List<NoteBase> elements = dbContext.Notes.Where(
            e => e.SectionId == section.Id
        ).OrderBy(e => e.OrdinalPosition).ToList();

        if (elements.Count != expectedCount)
        {
            return false;
        }

        List<int> ordinalPositions = [];
        foreach (NoteBase element in elements)
        {
            ordinalPositions.Add(element.OrdinalPosition);
        }

        for (int i = 0; i < expectedCount; i++ )
        {
            if (ordinalPositions[i] != i)
            {
                return false;
            }
        }

        return true;
    }
}