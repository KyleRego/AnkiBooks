using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class MarkdownContent : ArticleElementBase, IArticleContent
{
    [Required]
    public string? Text { get; set; }
}