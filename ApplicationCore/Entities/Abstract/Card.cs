using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using AnkiBooks.ApplicationCore.Enums;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class Card : EntityBase, ICard
{
    [Required]
    public string? DeckId { get; set; }

    [JsonIgnore]
    public Deck? Deck { get; set; }

    [NotMapped]
    public bool ShowingAnswer { get; set; } = false;

    public List<Repetition> Repetitions { get; set; } = [];

    [Required]
    public float EasinessFactor { get; set; } = 2.5F;

    [Required]
    public int InterRepetitionInterval { get; set; } = 0;

    // TODO: DRY this method call by putting it somewhere maybe in its own class
    [Required]
    public long DueAt { get; set; } = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

    public void UpdateSelfAfterRepetition(Grade grade, int successStreak)
    {
        if (grade == Grade.Good)
        {
            if (successStreak == 0)
            {
                InterRepetitionInterval = 1;
            }
            else if (successStreak == 1)
            {
                InterRepetitionInterval = 6;
            }
            else
            {
                InterRepetitionInterval = (int)Math.Round(InterRepetitionInterval * EasinessFactor);
            }
        }
        else
        {
            InterRepetitionInterval = 0;
            EasinessFactor = 0;
        }

        DueAt = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds() + InterRepetitionInterval * 86400;

        // If there were 6 grades, q could be 1 to 6 (see SM2 algorithm)
        // Since Anki Books uses 2 grades, take them to represent 2 and 5
        int q = grade == Grade.Bad ? 2 : 5;

        EasinessFactor += 0.1F - (5 - q) * (0.08F + (5 - q) * 0.02F);

        if (EasinessFactor < 1.3)
        {
            EasinessFactor = 1.3F;
        }
    }
}