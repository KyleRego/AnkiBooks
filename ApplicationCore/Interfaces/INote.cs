namespace AnkiBooks.ApplicationCore.Interfaces;

public interface INote : IEntityBase, IOrderedElement
{
    public string? SectionId { get; set; }
}