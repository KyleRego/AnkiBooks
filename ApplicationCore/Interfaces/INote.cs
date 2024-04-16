namespace AnkiBooks.ApplicationCore.Interfaces;

public interface INote : IEntityBase, IOrdinalChild
{
    public string? SectionId { get; set; }
}