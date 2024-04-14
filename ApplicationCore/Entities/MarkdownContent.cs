using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class MarkdownContent : ArticleContentBase, IArticleContent
{
    [Required]
    public string? Text { get; set; }
}