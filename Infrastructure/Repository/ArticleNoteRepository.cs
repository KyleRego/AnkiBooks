using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class ArticleNoteRepository(ApplicationDbContext dbContext)
                        : ArticleElementRepositoryBase<ArticleNoteBase>(dbContext)
{
    protected override void AddElementToDbContext(ArticleNoteBase element)
    {
        _dbContext.ArticleNotes.Add(element);
    }

    protected override void RemoveElementFromDbContext(ArticleNoteBase element)
    {
        _dbContext.ArticleNotes.Remove(element);
    }

    protected override int ArticleElementsCount(string articleId)
    {
        return _dbContext.ArticleNotes.Count(
            e => e.ArticleId == articleId
        );
    }

    protected override List<ArticleNoteBase> ElementsToShiftUpOnInsert(string articleId, int ordinalPosition)
    {
        return _dbContext.ArticleNotes.Where(
            e => e.ArticleId == articleId && e.OrdinalPosition >= ordinalPosition
        ).ToList();
    }

    protected override List<ArticleNoteBase> ElementsToShiftDownOnDelete(string articleId, int ordinalPosition)
    {
        return _dbContext.ArticleNotes.Where(
            e => e.ArticleId == articleId && e.OrdinalPosition >= ordinalPosition
        ).ToList();
    }

    protected override int CurrentOrdinalPositionOfUpdatedElement(ArticleNoteBase element)
    {
        return _dbContext.ArticleNotes.AsNoTracking().Single(e => e.Id == element.Id).OrdinalPosition;
    }

    protected override ArticleNoteBase CurrentVersionOfChangedElement(ArticleNoteBase changedElement)
    {
        return _dbContext.ArticleNotes.First(bn => bn.Id == changedElement.Id);
    }

    protected override List<ArticleNoteBase> ElementsToShiftDownOnMove(ArticleNoteBase movingElement, int origLowOrdPos, int newHighOrdPos)
    {
        return _dbContext.ArticleNotes.Where(
                    e => e.ArticleId == movingElement.ArticleId 
                    && e.OrdinalPosition > origLowOrdPos
                    && e.OrdinalPosition <= newHighOrdPos
                    && e.Id != movingElement.Id
                ).ToList();
    }

    protected override List<ArticleNoteBase> ElementsToShiftUpOnMove(ArticleNoteBase movingElement, int origHighOrdPos, int newLowOrdPos)
    {
        return _dbContext.ArticleNotes.Where(
                    e => e.ArticleId == movingElement.ArticleId
                    && e.OrdinalPosition < origHighOrdPos
                    && e.OrdinalPosition >= newLowOrdPos
                    && e.Id != movingElement.Id
                ).ToList();
    }
}