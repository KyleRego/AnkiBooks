using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IArticleElement
{
    public string? ArticleId { get; set; }

    public int OrdinalPosition { get; set; }

    public ICollection<Concept> Concepts { get; }
}