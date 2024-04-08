namespace AnkiBooks.ApplicationCore.Entities;

public class MarkdownContent : ArticleElementBase
{
    [Required]
    public string? Text { get; set; }
}