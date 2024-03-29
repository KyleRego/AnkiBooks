namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : ArticleElementBase
{
    [Required]
    public string? Text { get; set; }
}