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

    public void UpdateSelfForRepetition(Grade grade, int successStreak)
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

        // If there were 6 grades, qFactor would be 1-6 (see SM2 algorithm)
        // Since there are 2 grades, take them to represent 4 and 2
        int q = grade == Grade.Good ? 4 : 2;

        EasinessFactor += 0.1F - (5 - q) * (0.08F + (5 - q) * 0.02F);

        if (EasinessFactor < 1.3)
        {
            EasinessFactor = 1.3F;
        }
    }
}