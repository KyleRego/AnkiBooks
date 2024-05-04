using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : Card
{
    [Required]
    public string? Text { get; set; }
}