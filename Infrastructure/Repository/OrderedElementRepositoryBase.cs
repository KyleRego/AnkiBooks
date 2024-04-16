using System.Linq.Expressions;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public abstract class OrderedElementRepositoryBase<T>(ApplicationDbContext dbContext)
        : IOrderedElementRepository<T> where T : IOrdinalChild
{
    protected readonly ApplicationDbContext _dbContext = dbContext;
    protected abstract List<IOrdinalChild> GetAllOrdinalChildren(T element);
    protected abstract void AddElementToDbContext(T element);
    protected abstract void RemoveElementFromDbContext(T element);

    public async Task<T> InsertOrderedElementAsync(T newElement)
    {
        List<IOrdinalChild> ordinalChildren = GetAllOrdinalChildren(newElement);
        int ordinalElementsCount = ordinalChildren.Count;
        int insertPosition = newElement.OrdinalPosition;

        if (insertPosition > ordinalElementsCount || insertPosition < 0)
        {
            throw new OrdinalPositionException();
        }
        else
        {
            List<IOrdinalChild> siblingsToShift = ordinalChildren.Where(
                e => e.OrdinalPosition >= insertPosition
            ).ToList();

            foreach (IOrdinalChild e in siblingsToShift) { e.OrdinalPosition += 1; }
        }

        AddElementToDbContext(newElement);

        await _dbContext.SaveChangesAsync();

        return newElement;
    }

    public async Task DeleteOrderedElementAsync(T elementToDelete)
    {
        List<IOrdinalChild> ordinalChildren = GetAllOrdinalChildren(elementToDelete);
        int ordinalElementsCount = ordinalChildren.Count;
        int deletePosition = elementToDelete.OrdinalPosition;

        RemoveElementFromDbContext(elementToDelete);

        List<IOrdinalChild> siblingsToShift = ordinalChildren.Where(
            e => e.OrdinalPosition >= deletePosition
        ).ToList();

        foreach (IOrdinalChild e in siblingsToShift) { e.OrdinalPosition -= 1; }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> UpdateOrderedElementAsync(T curVersion, T newVersion)
    {
        int origOrdPos = curVersion.OrdinalPosition;
        int newOrdPos = newVersion.OrdinalPosition;

        if (newOrdPos == origOrdPos)
        {
            _dbContext.Entry(curVersion).State = EntityState.Detached;
            _dbContext.Entry(newVersion).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return newVersion;
        }
        else
        {
            List<IOrdinalChild> ordinalChildren = GetAllOrdinalChildren(curVersion);
            int ordinalElementsCount = ordinalChildren.Count;

            if (newOrdPos >= ordinalElementsCount || newOrdPos < 0)
            {
                throw new OrdinalPositionException();
            }
            else
            {
                curVersion.OrdinalPosition = ordinalElementsCount;

                if (newOrdPos > origOrdPos)
                {
                    List<IOrdinalChild> siblingsToShiftDown = ordinalChildren.Where(
                        e => e.OrdinalPosition > origOrdPos && e.OrdinalPosition <= newOrdPos
                    ).ToList();

                    foreach (IOrdinalChild e in siblingsToShiftDown) { e.OrdinalPosition -= 1; }

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    List<IOrdinalChild> siblingsToShiftUp = ordinalChildren.Where(
                        e => e.OrdinalPosition >= newOrdPos && e.OrdinalPosition < origOrdPos
                    ).ToList();

                    foreach (IOrdinalChild e in siblingsToShiftUp) { e.OrdinalPosition += 1; }

                    await _dbContext.SaveChangesAsync();
                }

                _dbContext.Entry(curVersion).State = EntityState.Detached;
                _dbContext.Entry(newVersion).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return newVersion;
            }
        }
    }
}