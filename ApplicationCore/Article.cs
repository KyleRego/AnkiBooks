namespace AnkiBooks.ApplicationCore;

public class Article(string title) : PrimaryKeyIdBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    public string? ParentArticleId { get; set; }
    public Article? ParentArticle { get; set; }

    public List<BasicNote> BasicNotes { get; set; } = [];

    public List<ClozeNote> ClozeNotes { get; set; } = [];
}