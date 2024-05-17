using System.ComponentModel.DataAnnotations.Schema;

using AnkiBooks.ApplicationCore.Enums;

namespace AnkiBooks.ApplicationCore.Entities;

public class Repetition : EntityBase
{
    [Required]
    [ForeignKey(nameof(Card))]
    public required string CardId { get; set; }

    public Card? Card { get; set; }

    [Required]
    public DateTimeOffset OccurredAt { get; set; }

    [Required]
    public Grade Grade { get; set; }
}