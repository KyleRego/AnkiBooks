using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class BasicNote : ArticleElementBase, IArticleNote
{
    [Required]
    public string? Front { get; set; }

    [Required]
    public string? Back { get; set; }
}