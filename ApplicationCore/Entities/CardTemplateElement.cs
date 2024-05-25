namespace AnkiBooks.ApplicationCore.Entities;

public class CardTemplateElement : EntityBase
{
    [Required]
    public string? CardTemplateId { get; set; }
    public CardTemplate? CardTemplate { get; set; }

    public string? NoteFieldId { get; set; }
    public NoteField? NoteField { get; set; }

    public string? Text { get; set; }

    public int OrdinalPosition { get; set; }
}