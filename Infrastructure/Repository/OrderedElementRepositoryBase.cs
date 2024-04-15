using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public abstract class OrderedElementRepositoryBase<T1, T2>(ApplicationDbContext dbContext) : IOrderedElementRepository<T2>
                                                    where T1: IOrderedElementsParent
                                                    where T2: IOrderedElement
{
    protected readonly ApplicationDbContext _dbContext = dbContext;
    protected abstract T1 GetParent(string parentId);
    protected abstract void AddElementToDbContext(T2 element);
    protected abstract void RemoveElementFromDbContext(T2 element);

    public async Task<T2> InsertOrderedElementAsync(T2 newElement)
    {
        string parentId = newElement.ParentId();
        T1 parent = GetParent(parentId);
        List<IOrderedElement> orderedElements = parent.OrderedElements();
        int orderedElementsCount = orderedElements.Count;
        int insertPosition = newElement.OrdinalPosition;

        if (insertPosition > orderedElementsCount || insertPosition < 0)
        {
            throw new OrdinalPositionException();
        }
        else
        {
            List<IOrderedElement> elementsToShift = orderedElements.Where(
                e => e.OrdinalPosition >= insertPosition
            ).ToList();

            foreach (IOrderedElement e in elementsToShift) { e.OrdinalPosition += 1; }
        }

        AddElementToDbContext(newElement);

        await _dbContext.SaveChangesAsync();

        return newElement;
    }

    public async Task DeleteOrderedElementAsync(T2 elementToDelete)
    {
        string parentId = elementToDelete.ParentId();
        T1 parent = GetParent(parentId);
        List<IOrderedElement> orderedElements = parent.OrderedElements();
        int orderedElementsCount = orderedElements.Count;
        int deletePosition = elementToDelete.OrdinalPosition;

        RemoveElementFromDbContext(elementToDelete);

        List<IOrderedElement> elementsToShift = orderedElements.Where(
            e => e.OrdinalPosition >= deletePosition
        ).ToList();

        foreach (IOrderedElement e in elementsToShift) { e.OrdinalPosition -= 1; }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<T2> UpdateOrderedElementAsync(T2 currentElement, T2 updateElement)
    {
        int origOrdPos = currentElement.OrdinalPosition;
        int newOrdPos = updateElement.OrdinalPosition;

        if (newOrdPos == origOrdPos)
        {
            _dbContext.Entry(currentElement).State = EntityState.Detached;
            _dbContext.Entry(updateElement).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return updateElement;
        }
        else
        {
            string parentId = updateElement.ParentId();
            T1 parent = GetParent(parentId);
            List<IOrderedElement> orderedElements = parent.OrderedElements();
            int articleElementsCount = orderedElements.Count;

            if (newOrdPos >= articleElementsCount || newOrdPos < 0)
            {
                throw new OrdinalPositionException();
            }
            else
            {
                currentElement.OrdinalPosition = articleElementsCount;

                if (newOrdPos > origOrdPos)
                {
                    List<IOrderedElement> elementsToShiftDown = orderedElements.Where(
                        e => e.OrdinalPosition > origOrdPos && e.OrdinalPosition <= newOrdPos
                    ).ToList();

                    foreach (IOrderedElement e in elementsToShiftDown) { e.OrdinalPosition -= 1; }

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    List<IOrderedElement> elementsToShiftUp = orderedElements.Where(
                        e => e.OrdinalPosition >= newOrdPos && e.OrdinalPosition < origOrdPos
                    ).ToList();

                    foreach (IOrderedElement e in elementsToShiftUp) { e.OrdinalPosition += 1; }

                    await _dbContext.SaveChangesAsync();
                }

                _dbContext.Entry(currentElement).State = EntityState.Detached;
                _dbContext.Entry(updateElement).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return updateElement;
            }
        }
    }
}