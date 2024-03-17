using AnkiBooks.Models.Enums;
using AnkiBooks.Models.Interfaces;

namespace AnkiBooks.Models;

/// <summary>
/// A note is a part of an article that correponds
/// to a note in Anki
/// </summary>
public class Note : IOrderedInArticle
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public NoteType Type { get; set; }

    [Required]
    public string? ArticleId { get; set; }
    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }

    public ICollection<Concept> Concepts { get; } = [];
}