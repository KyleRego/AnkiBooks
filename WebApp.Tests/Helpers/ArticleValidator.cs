using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.WebApp.Tests.Helpers;

public static class ArticleValidator
{
    public static bool CorrectElementsCountAndOrdinalPositions(
        ApplicationDbContext dbContext, Article article, int expectedCount)
    {
        List<ArticleElementBase> elements = dbContext.ArticleElements.Where(
            e => e.ArticleId == article.Id
        ).OrderBy(e => e.OrdinalPosition).ToList();

        if (elements.Count != expectedCount)
        {
            return false;
        }

        List<int> ordinalPositions = [];
        foreach (ArticleElementBase element in elements)
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