namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IContent : IEntityBase, IOrdinalChild
{
    public string? SectionId { get; set; }
}