using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class MarkdownContent : ArticleElement, IContent
{
    [Required]
    public string? Text { get; set; }
}