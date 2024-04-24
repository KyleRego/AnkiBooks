using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class BasicNote : ArticleElement
{
    [Required]
    public string? Front { get; set; }

    [Required]
    public string? Back { get; set; }
}