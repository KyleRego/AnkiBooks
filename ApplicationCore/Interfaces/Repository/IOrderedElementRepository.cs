using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IOrderedElementRepository<T> where T : IOrdinalChild
{
    Task<T> InsertOrderedElementAsync(T element);
    Task DeleteOrderedElementAsync(T element);
    Task<T> UpdateOrderedElementAsync(T currentElement, T updatedElement);
}