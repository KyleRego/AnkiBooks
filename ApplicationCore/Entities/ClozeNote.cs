using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : ArticleElement
{
    [Required]
    public string? Text { get; set; }
}