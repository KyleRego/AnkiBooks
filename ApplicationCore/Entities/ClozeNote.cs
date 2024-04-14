using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : ArticleNoteBase, IArticleNote
{
    [Required]
    public string? Text { get; set; }
}