using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class BasicNote : ArticleNoteBase, IArticleNote
{
    [Required]
    public string? Front { get; set; }

    [Required]
    public string? Back { get; set; }
}