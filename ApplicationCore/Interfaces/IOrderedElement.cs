namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IOrderedElement : IEntityBase
{
    public int OrdinalPosition { get; set; }

    public string ParentId();
}