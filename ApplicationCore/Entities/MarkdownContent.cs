using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

// TODO: Rename this to ArticleText
public class MarkdownContent : ArticleElement
{
    [Required]
    public string? Text { get; set; }
}