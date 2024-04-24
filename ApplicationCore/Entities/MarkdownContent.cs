namespace AnkiBooks.ApplicationCore.Entities;

public class MarkdownContent : ArticleElement
{
    [Required]
    public string? Text { get; set; }
}