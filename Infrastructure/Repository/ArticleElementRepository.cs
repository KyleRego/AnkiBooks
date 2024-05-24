using Microsoft.EntityFrameworkCore;

using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.Infrastructure.Repository;

public class ArticleElementRepository<T>(ApplicationDbContext dbContext)
            : IArticleElementRepository<T> where T : IArticleElement
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    /// <summary>
    /// Retrieves the siblings of element without including element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    protected List<IArticleElement> GetArticleElementSiblings(T element)
    {
        return _dbContext.ArticleElements
                .Where(el => el.ArticleId == element.ArticleId && el.Id != element.Id)
                .Cast<IArticleElement>().ToList();
    }

    public async Task<T> InsertAsync(T newElement)
    {
        List<IArticleElement> ordinalSiblings = GetArticleElementSiblings(newElement);

        int ordinalElementsCount = ordinalSiblings.Count;
        int insertPosition = newElement.OrdinalPosition;

        if (insertPosition > ordinalElementsCount || insertPosition < 0)
        {
            throw new OrdinalPositionException();
        }
        else
        {
            List<IArticleElement> siblingsToShift = ordinalSiblings.Where(
                e => e.OrdinalPosition >= insertPosition
            ).ToList();

            foreach (IArticleElement e in siblingsToShift)
            {
                e.OrdinalPosition += 1;
            }
        }

        _dbContext.Add(newElement);

        await _dbContext.SaveChangesAsync();

        return newElement;
    }

    public async Task DeleteAsync(T elementToDelete)
    {
        int deletePosition = elementToDelete.OrdinalPosition;

        List<IArticleElement> siblingsToShift = _dbContext.ArticleElements.Where(
            el => el.OrdinalPosition >= deletePosition && el.ArticleId == elementToDelete.ArticleId
        ).Cast<IArticleElement>().ToList();

        _dbContext.Remove(elementToDelete);

        foreach (IArticleElement e in siblingsToShift)
        {
            e.OrdinalPosition -= 1;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> UpdateAsync(T newVersion)
    {
        int origOrdPos = _dbContext.ArticleElements.AsNoTracking()
                            .First(el => el.Id == newVersion.Id).OrdinalPosition;

        int newOrdPos = newVersion.OrdinalPosition;

        if (newOrdPos == origOrdPos)
        {
            _dbContext.Entry(newVersion).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return newVersion;
        }
        else
        {
            List<IArticleElement> ordinalSiblings = GetArticleElementSiblings(newVersion);
            int ordinalElementsCount = ordinalSiblings.Count + 1;

            if (newOrdPos >= ordinalElementsCount || newOrdPos < 0)
            {
                throw new OrdinalPositionException();
            }
            else
            {
                if (newOrdPos > origOrdPos)
                {
                    List<IArticleElement> siblingsToShiftDown = ordinalSiblings.Where(
                        e => e.OrdinalPosition > origOrdPos && e.OrdinalPosition <= newOrdPos
                    ).ToList();

                    foreach (IArticleElement e in siblingsToShiftDown)
                    {
                        e.OrdinalPosition -= 1;
                    }
                }
                else
                {
                    List<IArticleElement> siblingsToShiftUp = ordinalSiblings.Where(
                        e => e.OrdinalPosition >= newOrdPos && e.OrdinalPosition < origOrdPos
                    ).ToList();

                    foreach (IArticleElement e in siblingsToShiftUp)
                    {
                        e.OrdinalPosition += 1;
                    }
                }

                _dbContext.Entry(newVersion).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                return newVersion;
            }
        }
    }
}