using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class ArticleContentRepository(ApplicationDbContext dbContext)
                        : ArticleElementRepositoryBase<ArticleContentBase>(dbContext)
{
    protected override void AddElementToDbContext(ArticleContentBase element)
    {
        _dbContext.ArticleContents.Add(element);
    }

    protected override void RemoveElementFromDbContext(ArticleContentBase element)
    {
        _dbContext.ArticleContents.Remove(element);
    }

    protected override int ArticleElementsCount(string articleId)
    {
        return _dbContext.ArticleContents.Count(
            e => e.ArticleId == articleId
        );
    }

    protected override List<ArticleContentBase> ElementsToShiftUpOnInsert(string articleId, int ordinalPosition)
    {
        return _dbContext.ArticleContents.Where(
            e => e.ArticleId == articleId && e.OrdinalPosition >= ordinalPosition
        ).ToList();
    }

    protected override List<ArticleContentBase> ElementsToShiftDownOnDelete(string articleId, int ordinalPosition)
    {
        return _dbContext.ArticleContents.Where(
            e => e.ArticleId == articleId && e.OrdinalPosition >= ordinalPosition
        ).ToList();
    }

    protected override int CurrentOrdinalPositionOfUpdatedElement(ArticleContentBase element)
    {
        return _dbContext.ArticleContents.AsNoTracking().Single(e => e.Id == element.Id).OrdinalPosition;
    }

    protected override ArticleContentBase CurrentVersionOfChangedElement(ArticleContentBase changedElement)
    {
        return _dbContext.ArticleContents.First(bn => bn.Id == changedElement.Id);
    }

    protected override List<ArticleContentBase> ElementsToShiftDownOnMove(ArticleContentBase movingElement, int origLowOrdPos, int newHighOrdPos)
    {
        return _dbContext.ArticleContents.Where(
                    e => e.ArticleId == movingElement.ArticleId 
                    && e.OrdinalPosition > origLowOrdPos
                    && e.OrdinalPosition <= newHighOrdPos
                    && e.Id != movingElement.Id
                ).ToList();
    }

    protected override List<ArticleContentBase> ElementsToShiftUpOnMove(ArticleContentBase movingElement, int origHighOrdPos, int newLowOrdPos)
    {
        return _dbContext.ArticleContents.Where(
                    e => e.ArticleId == movingElement.ArticleId
                    && e.OrdinalPosition < origHighOrdPos
                    && e.OrdinalPosition >= newLowOrdPos
                    && e.Id != movingElement.Id
                ).ToList();
    }
}