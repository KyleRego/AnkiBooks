namespace AnkiBooks.Models;

public interface IHasConcepts
{
    public ICollection<Concept> Concepts { get; }
}