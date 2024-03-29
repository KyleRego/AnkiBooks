namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IArticleElement
{
    public string? ArticleId { get; set; }

    public int OrdinalPosition { get; set; }   
}