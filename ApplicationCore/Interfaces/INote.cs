namespace AnkiBooks.ApplicationCore.Interfaces;

public interface INote : IEntityBase
{
    public string? SectionId { get; set; }

    public int OrdinalPosition { get; set; }
}