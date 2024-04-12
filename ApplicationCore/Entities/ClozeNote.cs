using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : ArticleElementBase, IArticleNote
{
    [Required]
    public string? Text { get; set; }
}