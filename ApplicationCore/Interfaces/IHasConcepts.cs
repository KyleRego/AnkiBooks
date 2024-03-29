namespace AnkiBooks.ApplicationCore;

public interface IHasConcepts
{
    public ICollection<Concept> Concepts { get; }
}