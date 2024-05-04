using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class BasicNote : Card
{
    [Required]
    public string? Front { get; set; }

    [Required]
    public string? Back { get; set; }
}