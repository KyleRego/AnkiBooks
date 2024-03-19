namespace AnkiBooks.Models;

public class Article(string title) : PrimaryKeyIdBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    public string? BookId { get; set; }
    public Book? Book { get; set; }

    public ICollection<ArticleElement> Elements { get; } = [];
}