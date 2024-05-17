using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class Card : EntityBase, ICard
{
    [Required]
    public string? DeckId { get; set; }

    [JsonIgnore]
    public Deck? Deck { get; set; }

    public IEnumerable<Repetition> Repetitions { get; set; } = [];

    [Required]
    public float EasinessFactor { get; set; } = 2.5F;

    [Required]
    public int InterRepetitionInterval { get; set; } = 0;

    [Required]
    public int SuccessfulRecallStreak { get; set; } = 0;

    public DateTimeOffset? LastReviewedAt { get; set; }
}