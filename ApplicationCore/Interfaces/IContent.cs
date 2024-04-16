namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IContent : IEntityBase
{
    public string? SectionId { get; set; }

    public int OrdinalPosition { get; set; }
}