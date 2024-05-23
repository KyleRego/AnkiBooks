using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Repository;

public interface IOrderedElementRepository<T> where T : IOrdinalChild
{
    Task<T> InsertOrderedElementAsync(T element);

    Task DeleteOrderedElementAsync(T element);

    Task<T> UpdateOrderedElementAsync(T element);
}