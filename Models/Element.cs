using AnkiBooks.Models.Enums;
using AnkiBooks.Models.Interfaces;

namespace AnkiBooks.Models;

/// <summary>
/// An element is a part of an article that does not
/// correspond to a note in Anki.
/// </summary>
public class Element(ElementType type) : IOrderedInArticle
{
    [Key]
    public string Id = Guid.NewGuid().ToString();

    [Required]
    public ElementType Type = type;

    [Required]
    public string? ArticleId { get; set; }
    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }

    public ICollection<Concept> Concepts { get; } = [];
}