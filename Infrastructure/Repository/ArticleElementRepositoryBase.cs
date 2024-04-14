using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public abstract class ArticleElementRepositoryBase<T>(ApplicationDbContext dbContext) where T : IArticleElement
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    protected abstract int ArticleElementsCount(string articleId);

    protected abstract void AddElementToDbContext(T element);

    protected abstract void RemoveElementFromDbContext(T element);

    protected abstract List<T> ElementsToShiftUpOnInsert(string articleId, int ordinalPosition);

    protected abstract List<T> ElementsToShiftDownOnDelete(string articleId, int ordinalPosition);

    protected abstract List<T> ElementsToShiftUpOnMove(T element, int origHighOrdPos, int newLowOrdPos);

    protected abstract List<T> ElementsToShiftDownOnMove(T element, int origLowOrdPos, int newHighOrdPos);

    protected abstract int CurrentOrdinalPositionOfUpdatedElement(T element);

    protected abstract T CurrentVersionOfChangedElement(T element);

    public async Task<T> InsertArticleElementAsync(T newElement)
    {
        ArgumentNullException.ThrowIfNull(newElement.ArticleId);
        int articleElementsCount = ArticleElementsCount(newElement.ArticleId);

        if (newElement.OrdinalPosition > articleElementsCount || newElement.OrdinalPosition < 0)
        {
            throw new OrdinalPositionException();
        }
        else
        {
            List<T> elementsToShift = ElementsToShiftUpOnInsert(newElement.ArticleId, newElement.OrdinalPosition);

            foreach (T e in elementsToShift) { e.OrdinalPosition += 1; }
        }

        AddElementToDbContext(newElement);

        await _dbContext.SaveChangesAsync();

        return newElement;
    }

    public async Task DeleteArticleElementAsync(T elementToDelete)
    {
        ArgumentNullException.ThrowIfNull(elementToDelete.ArticleId);

        int deletedOrdinalPosition = elementToDelete.OrdinalPosition;

        RemoveElementFromDbContext(elementToDelete);

        List<T> elementsToShift = ElementsToShiftDownOnDelete(elementToDelete.ArticleId, deletedOrdinalPosition);

        foreach (T e in elementsToShift) { e.OrdinalPosition -= 1; }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> UpdateArticleElementAsync(T changedElement)
    {
        ArgumentNullException.ThrowIfNull(changedElement.ArticleId);

        int origOrdPos = CurrentOrdinalPositionOfUpdatedElement(changedElement);
        int newOrdPos = changedElement.OrdinalPosition;

        if (newOrdPos == origOrdPos)
        {
            _dbContext.Entry(changedElement).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return changedElement;
        }
        else
        {
            int articleElementsCount = ArticleElementsCount(changedElement.ArticleId);

            if (newOrdPos >= articleElementsCount || changedElement.OrdinalPosition < 0)
            {
                throw new OrdinalPositionException();
            }
            else
            {
                T existingElement = CurrentVersionOfChangedElement(changedElement);
                existingElement.OrdinalPosition = articleElementsCount;

                if (newOrdPos > origOrdPos)
                {
                    List<T> elementsToShiftDown = ElementsToShiftDownOnMove(changedElement, origOrdPos, newOrdPos);

                    foreach (T e in elementsToShiftDown) { e.OrdinalPosition -= 1; }

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    List<T> elementsToShiftUp = ElementsToShiftUpOnMove(changedElement, origOrdPos, newOrdPos);

                    foreach (T e in elementsToShiftUp) { e.OrdinalPosition += 1; }

                    await _dbContext.SaveChangesAsync();
                }

                _dbContext.Entry(existingElement).State = EntityState.Detached;
                _dbContext.Entry(changedElement).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return changedElement;
            }
        }
    }
}