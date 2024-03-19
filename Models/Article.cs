namespace AnkiBooks.Models;

public class Article(string title) : PrimaryKeyIdBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    [Required]
    public string? BookId { get; set; }
    public Book? Book { get; set; }

    public ICollection<ArticleElement> Elements { get; } = [];
}