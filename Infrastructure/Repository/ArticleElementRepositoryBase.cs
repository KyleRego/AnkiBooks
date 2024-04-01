using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public abstract class ArticleElementRepositoryBase(ApplicationDbContext dbContext)
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<ArticleElementBase> InsertArticleElementAsync(ArticleElementBase newElement)
    {
        int articleElementsCount = _dbContext.ArticleElements.Count(
            e => e.ArticleId == newElement.ArticleId
        );

        if (newElement.OrdinalPosition > articleElementsCount || newElement.OrdinalPosition < 0)
        {
            throw new OrdinalPositionException();
        }
        else
        {
            List<ArticleElementBase> elementsToShift = _dbContext.ArticleElements.Where(
                e => e.ArticleId == newElement.ArticleId && e.OrdinalPosition >= newElement.OrdinalPosition
            ).ToList();

            foreach (ArticleElementBase e in elementsToShift) { e.OrdinalPosition += 1; }
        }

        _dbContext.ArticleElements.Add(newElement);

        await _dbContext.SaveChangesAsync();

        return newElement;
    }

    public async Task DeleteArticleElementAsync(ArticleElementBase elementToDelete)
    {
        int deletedOrdinalPosition = elementToDelete.OrdinalPosition;

        _dbContext.ArticleElements.Remove(elementToDelete);

        List<ArticleElementBase> elementsToShift = _dbContext.ArticleElements.Where(
            e => e.ArticleId == elementToDelete.ArticleId && e.OrdinalPosition > deletedOrdinalPosition
        ).ToList();

        foreach (ArticleElementBase e in elementsToShift) { e.OrdinalPosition -= 1; }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<ArticleElementBase> UpdateArticleElementAsync(ArticleElementBase changedElement)
    {
        int originalOrdinalPosition = _dbContext.ArticleElements.AsNoTracking().Single(e => e.Id == changedElement.Id).OrdinalPosition;
        int newOrdinalPosition = changedElement.OrdinalPosition;

        if (newOrdinalPosition == originalOrdinalPosition)
        {
            _dbContext.Entry(changedElement).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return changedElement;
        }
        else
        {
            int articleElementsCount = _dbContext.ArticleElements.AsNoTracking().Count(
                e => e.ArticleId == changedElement.ArticleId
            );

            if (newOrdinalPosition >= articleElementsCount || changedElement.OrdinalPosition < 0)
            {
                throw new OrdinalPositionException();
            }
            else
            {
                ArticleElementBase existingElement = _dbContext.ArticleElements.First(bn => bn.Id == changedElement.Id);
                existingElement.OrdinalPosition = articleElementsCount;

                if (newOrdinalPosition > originalOrdinalPosition)
                {
                    List<ArticleElementBase> elementsToShiftDown = _dbContext.ArticleElements.Where(
                        e => e.ArticleId == changedElement.ArticleId 
                        && e.OrdinalPosition > originalOrdinalPosition
                        && e.OrdinalPosition <= newOrdinalPosition
                        && e.Id != changedElement.Id
                    ).ToList();

                    foreach (ArticleElementBase e in elementsToShiftDown) { e.OrdinalPosition -= 1; }

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    List<ArticleElementBase> elementsToShiftUp = _dbContext.ArticleElements.Where(
                        e => e.ArticleId == changedElement.ArticleId
                        && e.OrdinalPosition < originalOrdinalPosition
                        && e.OrdinalPosition >= newOrdinalPosition
                        && e.Id != changedElement.Id
                    ).ToList();

                    foreach (ArticleElementBase e in elementsToShiftUp) { e.OrdinalPosition += 1; }

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