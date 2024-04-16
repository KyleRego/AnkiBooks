using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class MarkdownContent : ContentBase, IContent
{
    [Required]
    public string? Text { get; set; }
}