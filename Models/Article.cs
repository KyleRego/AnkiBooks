namespace AnkiBooks.Models;

public class Article(string title) : PrimaryKeyIdBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    public string? ParentArticleId { get; set; }
    public Article? ParentArticle { get; set; }

    // public ICollection<ArticleElement> Elements { get; set; } = [];

    public ICollection<BasicNote> BasicNotes { get; set; } = [];
}