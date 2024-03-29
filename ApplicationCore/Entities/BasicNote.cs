namespace AnkiBooks.ApplicationCore.Entities;

public class BasicNote : ArticleElementBase
{
    [Required]
    public string? Front { get; set; }

    [Required]
    public string? Back { get; set; }
}