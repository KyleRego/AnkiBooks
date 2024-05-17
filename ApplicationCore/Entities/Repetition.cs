using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using AnkiBooks.ApplicationCore.Enums;

namespace AnkiBooks.ApplicationCore.Entities;

public class Repetition : EntityBase
{
    [Required]
    [ForeignKey(nameof(Card))]
    public required string CardId { get; set; }

    [JsonIgnore]
    public Card? Card { get; set; }

    // TODO: Research best way to calculate an integer time; maybe move to EntityBase as CreatedAt
    [Required]
    public long OccurredAt { get; set; } = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

    [Required]
    public Grade Grade { get; set; }
}