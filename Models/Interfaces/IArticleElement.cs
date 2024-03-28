namespace AnkiBooks.Models.Interfaces;

public interface IArticleElement
{
    public string? ArticleId { get; set; }

    public int OrdinalPosition { get; set; }   
}