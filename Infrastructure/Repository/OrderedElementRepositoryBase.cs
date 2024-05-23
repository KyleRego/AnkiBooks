using System.Linq.Expressions;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public abstract class OrderedElementRepositoryBase<T>(ApplicationDbContext dbContext)
        : IOrderedElementRepository<T> where T : IOrdinalChild
{
    protected readonly ApplicationDbContext _dbContext = dbContext;

    /// <summary>
    /// Retrieves the ordered siblings of element without including element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    protected abstract List<IOrdinalChild> GetAllOrdinalSiblings(T element);

    protected abstract int GetOriginalPosition(string elementId);

    public async Task<T> InsertOrderedElementAsync(T newElement)
    {
        List<IOrdinalChild> ordinalSiblings = GetAllOrdinalSiblings(newElement);
        int ordinalElementsCount = ordinalSiblings.Count;
        int insertPosition = newElement.OrdinalPosition;

        if (insertPosition > ordinalElementsCount || insertPosition < 0)
        {
            throw new OrdinalPositionException();
        }
        else
        {
            List<IOrdinalChild> siblingsToShift = ordinalSiblings.Where(
                e => e.OrdinalPosition >= insertPosition
            ).ToList();

            foreach (IOrdinalChild e in siblingsToShift) { e.OrdinalPosition += 1; }
        }

        _dbContext.Add(newElement);

        await _dbContext.SaveChangesAsync();

        return newElement;
    }

    public async Task DeleteOrderedElementAsync(T elementToDelete)
    {
        List<IOrdinalChild> ordinalSiblings = GetAllOrdinalSiblings(elementToDelete);

        int deletePosition = elementToDelete.OrdinalPosition;

        _dbContext.Remove(elementToDelete);

        List<IOrdinalChild> siblingsToShift = ordinalSiblings.Where(
            e => e.OrdinalPosition >= deletePosition
        ).ToList();

        foreach (IOrdinalChild e in siblingsToShift) { e.OrdinalPosition -= 1; }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> UpdateOrderedElementAsync(T newVersion)
    {
        int origOrdPos = GetOriginalPosition(newVersion.Id);
        int newOrdPos = newVersion.OrdinalPosition;

        if (newOrdPos == origOrdPos)
        {
            _dbContext.Entry(newVersion).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return newVersion;
        }
        else
        {
            List<IOrdinalChild> ordinalSiblings = GetAllOrdinalSiblings(newVersion);
            int ordinalElementsCount = ordinalSiblings.Count + 1;

            if (newOrdPos >= ordinalElementsCount || newOrdPos < 0)
            {
                throw new OrdinalPositionException();
            }
            else
            {
                if (newOrdPos > origOrdPos)
                {
                    List<IOrdinalChild> siblingsToShiftDown = ordinalSiblings.Where(
                        e => e.OrdinalPosition > origOrdPos && e.OrdinalPosition <= newOrdPos
                    ).ToList();

                    foreach (IOrdinalChild e in siblingsToShiftDown) { e.OrdinalPosition -= 1; }
                }
                else
                {
                    List<IOrdinalChild> siblingsToShiftUp = ordinalSiblings.Where(
                        e => e.OrdinalPosition >= newOrdPos && e.OrdinalPosition < origOrdPos
                    ).ToList();

                    foreach (IOrdinalChild e in siblingsToShiftUp) { e.OrdinalPosition += 1; }
                }

                _dbContext.Entry(newVersion).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return newVersion;
            }
        }
    }
}