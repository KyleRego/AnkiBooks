namespace AnkiBooks.Models;

public abstract class ArticleElement : PrimaryKeyIdBase
{
    [Required]
    public string? ArticleId { get; set; }

    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }
}