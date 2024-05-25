namespace AnkiBooks.ApplicationCore.Entities;

public class CardTemplate : EntityBase
{
    [Required]
    public string? CardTypeId { get; set; }

    public CardType? CardType { get; set; }

    public List<CardTemplateElement> Elements { get; set; } = [];
}