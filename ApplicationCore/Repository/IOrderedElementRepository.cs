using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IOrderedElementRepository<T> where T : IOrdinalChild
{
    Task<T> InsertOrderedElementAsync(T element);
    Task DeleteOrderedElementAsync(T element);
    // TODO: Pull complexity of currentElement into this
    // Only provide one argument, the updatedElement
    Task<T> UpdateOrderedElementAsync(T currentElement, T updatedElement);
}