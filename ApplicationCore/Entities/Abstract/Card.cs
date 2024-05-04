using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class Card : EntityBase, ICard
{
    [Required]
    public string? DeckId { get; set; }

    [JsonIgnore]
    public Deck? Deck { get; set; }
}