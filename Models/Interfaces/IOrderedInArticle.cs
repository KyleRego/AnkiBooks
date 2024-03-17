namespace AnkiBooks.Models.Interfaces;

public interface IOrderedInArticle
{
    public string? ArticleId { get; set; }

    public Article? Article { get; set; }

    public int OrdinalPosition { get; set; }
}