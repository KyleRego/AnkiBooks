namespace AnkiBooks.ApplicationCore.Entities;

public class CardType : EntityBase
{
    public required string Name { get; set; }

    [Required]
    public string? NoteTypeId { get; set; }

    public NoteType? NoteType { get; set; }

    [Required]
    public required CardTemplate FrontTemplate { get; set; }

    [Required]
    public required CardTemplate BackTemplate { get; set; }
}