namespace AnkiBooks.ApplicationCore.Entities;

public class NoteField : EntityBase
{
    [Required]
    public string? NoteTypeId { get; set; }

    public NoteType? NoteType { get; set; }

    [Required]
    public required string Name { get; set; }
}